using System;
using System.Collections.Generic;
using System.Linq;

public class UIManager
{
    private const char HORIZONTAL = '─';
    private const char VERTICAL = '│';
    private const char TOP_LEFT = '┌';
    private const char TOP_RIGHT = '┐';
    private const char BOTTOM_LEFT = '└';
    private const char BOTTOM_RIGHT = '┘';
    private const char T_DOWN = '┬';
    private const char T_UP = '┴';
    private const char T_LEFT = '┤';
    private const char T_RIGHT = '├';
    private const char CROSS = '┼';

    private static int InnerWidth(int boxWidth) => boxWidth - 4;

    private static void WriteBoxLine(int boxWidth, string content)
    {
        int inner = InnerWidth(boxWidth);
        if (content.Length > inner)
            content = content[..inner];

        Console.Write(VERTICAL);
        Console.Write(' ');
        Console.Write(content);
        Console.Write(new string(' ', inner - content.Length));
        Console.Write(' ');
        Console.WriteLine(VERTICAL);
    }

    private static void WriteBoxLine(int boxWidth, int contentLength, Action writeContent)
    {
        int inner = InnerWidth(boxWidth);
        int padding = Math.Max(0, inner - contentLength);

        Console.Write(VERTICAL);
        Console.Write(' ');
        writeContent();
        Console.Write(new string(' ', padding));
        Console.Write(' ');
        Console.WriteLine(VERTICAL);
    }

    public static void ClearScreen()
    {
        Console.Clear();
    }

    public static void DrawBox(string title, int width, int height, string content = "")
    {
        DrawTopBorder(title, width);
        
        if (!string.IsNullOrEmpty(content))
        {
            var lines = content.Split('\n');
            for (int i = 0; i < height - 2; i++)
            {
                if (i < lines.Length)
                    WriteBoxLine(width, lines[i]);
                else
                    WriteBoxLine(width, string.Empty);
            }
        }
        else
        {
            for (int i = 0; i < height - 2; i++)
            {
                Console.Write(VERTICAL);
                Console.Write(new string(' ', width - 2));
                Console.WriteLine(VERTICAL);
            }
        }

        DrawBottomBorder(width);
    }

    public static void DrawTopBorder(string title, int width)
    {
        Console.Write(TOP_LEFT);
        if (!string.IsNullOrEmpty(title))
        {
            int paddingLeft = (width - title.Length - 4) / 2;
            int paddingRight = width - title.Length - 4 - paddingLeft;
            Console.Write(new string(HORIZONTAL, paddingLeft));
            Console.Write(" " + title + " ");
            Console.Write(new string(HORIZONTAL, paddingRight));
        }
        else
        {
            Console.Write(new string(HORIZONTAL, width - 2));
        }
        Console.WriteLine(TOP_RIGHT);
    }

    public static void DrawBottomBorder(int width)
    {
        Console.Write(BOTTOM_LEFT);
        Console.Write(new string(HORIZONTAL, width - 2));
        Console.WriteLine(BOTTOM_RIGHT);
    }

    public static void DrawSeparator(int width)
    {
        Console.Write(T_RIGHT);
        Console.Write(new string(HORIZONTAL, width - 2));
        Console.WriteLine(T_LEFT);
    }

    public static void DrawHealthBar(int current, int max, int width = 20)
    {
        if (max <= 0)
        {
            Console.Write("[" + new string('░', width) + "]");
            return;
        }

        double ratio = (double)current / max;
        int filled = (int)Math.Round(ratio * width);
        filled = Math.Clamp(filled, 0, width);

        Console.ForegroundColor = GetHealthColor(ratio);
        Console.Write("[");
        Console.Write(new string('█', filled));
        Console.Write(new string('░', width - filled));
        Console.Write("]");
        Console.ResetColor();

        Console.Write($" {current}/{max}");
    }

    public static ConsoleColor GetHealthColor(double ratio)
    {
        if (ratio > 0.6) return ConsoleColor.Green;
        if (ratio > 0.3) return ConsoleColor.Yellow;
        return ConsoleColor.Red;
    }

    public static void DrawPlayerStats(Player player)
    {
        Console.Clear();
        int boxWidth = 60;

        DrawTopBorder("СТАТИСТИКА ГЕРОЯ", boxWidth);

        WriteBoxLine(boxWidth, $"Имя: {player.Name}");
        WriteBoxLine(boxWidth, $"Уровень: {player.Level}");

        string healthLabel = "Здоровье: ";
        int healthBarLength = 2 + 22 + 1 + $"{player.Health}/{player.MaxHealth}".Length;
        WriteBoxLine(boxWidth, healthLabel.Length + healthBarLength, () =>
        {
            Console.Write(healthLabel);
            DrawHealthBar(player.Health, player.MaxHealth, 22);
        });

        WriteBoxLine(boxWidth, $"Оружие: {player.EquippedWeapon.GetDescription()}");
        WriteBoxLine(boxWidth, $"Урон: {player.EquippedWeapon.GetDamage()}");

        DrawBottomBorder(boxWidth);
    }

    public static void DrawMap(WorldMap map)
    {
        int boxWidth = 74; 
        Console.WriteLine();
        DrawTopBorder(map.MapName ?? "Карта", boxWidth);

        var locationsByLevel = map.Locations.Values.GroupBy(l => l.Level).OrderByDescending(g => g.Key);
        var travelOptions = map.GetTravelOptions();

        foreach (var levelGroup in locationsByLevel)
        {
            var nodes = levelGroup.ToList();
            
            int totalTextLength = nodes.Sum(n => $"[{n.Name}]".Length);
            int spacing = 6;
            int totalContentWidth = totalTextLength + (nodes.Count - 1) * spacing;
            
            int inner = InnerWidth(boxWidth);
            int leftPad = Math.Max(0, (inner - totalContentWidth) / 2);

            WriteBoxLine(boxWidth, leftPad + totalContentWidth, () => 
            {
                Console.Write(new string(' ', leftPad));
                for (int i = 0; i < nodes.Count; i++)
                {
                    var node = nodes[i];
                    string text = $"[{node.Name}]";
                    
                    if (map.CurrentLocation?.Id == node.Id)
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    else if (travelOptions.Any(o => o.Id == node.Id))
                        Console.ForegroundColor = ConsoleColor.Green;
                    else if (map.HasVisited(node))
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    else
                        Console.ForegroundColor = ConsoleColor.White;

                    Console.Write(text);
                    Console.ResetColor();

                    if (i < nodes.Count - 1)
                        Console.Write(new string(' ', spacing));
                }
            });
        
            if (levelGroup.Key > locationsByLevel.Last().Key) 
            {
                WriteBoxLine(boxWidth, "");
            }
        }

        DrawSeparator(boxWidth);

        string locationName = map.CurrentLocation?.Name ?? "Неизвестно";
        WriteBoxLine(boxWidth, $"Вы находитесь: {locationName}");
        DrawSeparator(boxWidth);

        if (travelOptions.Count == 0)
        {
            WriteBoxLine(boxWidth, "Дальше пути нет.");
        }
        else
        {
            for (int i = 0; i < travelOptions.Count; i++)
            {
                var location = travelOptions[i];
                
                string arrow = location.Level <= map.CurrentLocation!.Level ? "← " : "→ ";
                bool visited = map.HasVisited(location);
                
                string optionText = $"{i + 1}) {arrow}{location.Name}";
                WriteBoxLine(boxWidth, optionText.Length, () =>
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{i + 1})");
                    Console.ResetColor();
                    Console.Write($" {arrow}");
                    
                    if (visited)
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    else
                        Console.ForegroundColor = ConsoleColor.Green;
                        
                    Console.Write(location.Name);
                    Console.ResetColor();
                });
            }
        }

        DrawBottomBorder(boxWidth);
    }

    public static void DrawBattleInterface(Player player, Boss boss)
    {
        Console.Clear();
        int boxWidth = 70;

        DrawTopBorder($"БОЙ: {boss.Name}", boxWidth);

        WriteBoxLine(boxWidth, $"Противник: {boss.Name}");

        string bossHealthLabel = "Здоровье босса: ";
        int bossBarLength = 2 + 22 + 1 + $"{boss.Health}/{boss.GetMaxHealth()}".Length;
        WriteBoxLine(boxWidth, bossHealthLabel.Length + bossBarLength, () =>
        {
            Console.Write(bossHealthLabel);
            DrawHealthBar(boss.Health, boss.GetMaxHealth(), 22);
        });

        string playerHealthLabel = "Ваше здоровье: ";
        int playerBarLength = 2 + 22 + 1 + $"{player.Health}/{player.MaxHealth}".Length;
        WriteBoxLine(boxWidth, playerHealthLabel.Length + playerBarLength, () =>
        {
            Console.Write(playerHealthLabel);
            DrawHealthBar(player.Health, player.MaxHealth, 22);
        });

        DrawSeparator(boxWidth);

        WriteBoxLine(boxWidth, "Части тела:");

        for (int i = 0; i < boss.BossBodyParts.Count; i++)
        {
            var part = boss.BossBodyParts[i];
            string partText = $"[{i + 1}] {part.Name} (x{part.DamageMultiplier})";
            WriteBoxLine(boxWidth, partText.Length, () =>
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"[{i + 1}]");
                Console.ResetColor();
                Console.Write($" {part.Name} (x{part.DamageMultiplier})");
            });
        }

        DrawBottomBorder(boxWidth);
    }

    public static void DrawEnemyEncounter(Enemy enemy, Player player)
    {
        Console.Clear();
        int boxWidth = 60;

        DrawTopBorder("ВСТРЕЧА С ВРАГОМ", boxWidth);

        WriteBoxLine(boxWidth, $"Враг: {enemy.Name}");

        string enemyHealthLabel = "Здоровье врага: ";
        int enemyBarLength = 2 + 18 + 1 + $"{enemy.Health}/100".Length;
        WriteBoxLine(boxWidth, enemyHealthLabel.Length + enemyBarLength, () =>
        {
            Console.Write(enemyHealthLabel);
            DrawHealthBar(enemy.Health, 100, 18);
        });

        string playerHealthLabel = "Ваше здоровье: ";
        int playerBarLength = 2 + 18 + 1 + $"{player.Health}/{player.MaxHealth}".Length;
        WriteBoxLine(boxWidth, playerHealthLabel.Length + playerBarLength, () =>
        {
            Console.Write(playerHealthLabel);
            DrawHealthBar(player.Health, player.MaxHealth, 18);
        });

        DrawBottomBorder(boxWidth);
    }

    public static void ShowMessage(string title, string message, int width = 60)
    {
        Console.WriteLine();
        DrawTopBorder(title, width);
        
        var lines = message.Split('\n');
        for (int i = 0; i < Math.Max(lines.Length, 3); i++)
        {
            if (i < lines.Length)
                WriteBoxLine(width, lines[i]);
            else
                WriteBoxLine(width, string.Empty);
        }
        
        DrawBottomBorder(width);
        Console.WriteLine();
    }

    public static void DrawMenu(string title, List<(string label, ConsoleColor color)> options, int width = 50)
    {
        Console.WriteLine();
        DrawTopBorder(title, width);

        foreach (var (label, color) in options)
        {
            WriteBoxLine(width, label.Length, () =>
            {
                Console.ForegroundColor = color;
                Console.Write(label);
                Console.ResetColor();
            });
        }

        DrawBottomBorder(width);
        Console.WriteLine();
    }

    public static void DrawMainMenu()
    {
        Console.Clear();
        int width = 50;
        
        DrawTopBorder("╔═══════════════════╗", width);
        const string titleLine = "   ДРЕВНЯЯ ТЬМА   ";
        WriteBoxLine(width, titleLine.Length, () =>
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(titleLine);
            Console.ResetColor();
        });
        
        DrawSeparator(width);

        var options = new List<(string, ConsoleColor)>
        {
            ("[1] Новая игра", ConsoleColor.Green),
            ("[2] Загрузить игру", ConsoleColor.Yellow),
            ("[3] Параметры", ConsoleColor.Cyan),
            ("[0] Выход", ConsoleColor.Red)
        };

        DrawMenu("", options, width);
    }
}