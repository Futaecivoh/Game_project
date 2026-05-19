using System;
using System.Collections.Generic;

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
                {
                    Console.Write(VERTICAL + " ");
                    Console.Write(lines[i].PadRight(width - 4));
                    Console.WriteLine(" " + VERTICAL);
                }
                else
                {
                    Console.Write(VERTICAL + " ");
                    Console.Write(new string(' ', width - 4));
                    Console.WriteLine(" " + VERTICAL);
                }
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
        
        Console.Write(VERTICAL + " ");
        Console.Write($"Имя: {player.Name}".PadRight(boxWidth - 4));
        Console.WriteLine(" " + VERTICAL);

        Console.Write(VERTICAL + " ");
        Console.Write($"Уровень: {player.Level}".PadRight(boxWidth - 4));
        Console.WriteLine(" " + VERTICAL);

        Console.Write(VERTICAL + " ");
        Console.Write("Здоровье: ".PadRight(boxWidth - 4 - 25));
        DrawHealthBar(player.Health, player.MaxHealth, 22);
        Console.WriteLine(" " + VERTICAL);

        Console.Write(VERTICAL + " ");
        Console.Write($"Оружие: {player.EquippedWeapon.GetDescription()}".PadRight(boxWidth - 4));
        Console.WriteLine(" " + VERTICAL);

        Console.Write(VERTICAL + " ");
        Console.Write($"Урон: {player.EquippedWeapon.GetDamage()}".PadRight(boxWidth - 4));
        Console.WriteLine(" " + VERTICAL);

        DrawBottomBorder(boxWidth);
    }

    public static void DrawMap(WorldMap map)
    {
        int boxWidth = 80;
        Console.WriteLine();
        DrawTopBorder($"КАРТА: {map.MapName}", boxWidth);

        Console.Write(VERTICAL + " ");
        Console.Write("Текущая локация: ".PadRight(boxWidth - 4 - 30));
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(map.CurrentLocation?.Name ?? "Неизвестно");
        Console.ResetColor();
        Console.WriteLine(" " + VERTICAL);

        Console.Write(VERTICAL + " ");
        Console.Write($"Тип: {map.CurrentLocation?.Type ?? "N/A"}".PadRight(boxWidth - 4));
        Console.WriteLine(" " + VERTICAL);

        DrawSeparator(boxWidth);

        Console.Write(VERTICAL + " Доступные локации:".PadRight(boxWidth - 1));
        Console.WriteLine(VERTICAL);

        if (map.CurrentLocation?.ConnectedLocations.Count == 0)
        {
            Console.Write(VERTICAL + " ");
            Console.Write("Дальше пути нет!".PadRight(boxWidth - 4));
            Console.WriteLine(" " + VERTICAL);
        }
        else
        {
            for (int i = 0; i < map.CurrentLocation?.ConnectedLocations.Count; i++)
            {
                var location = map.CurrentLocation.ConnectedLocations[i];
                Console.Write(VERTICAL + " ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"[{i + 1}]");
                Console.ResetColor();
                Console.Write($" -> {location.Name} ({location.Type})".PadRight(boxWidth - 10));
                Console.WriteLine(VERTICAL);
            }
        }

        DrawBottomBorder(boxWidth);
    }

    public static void DrawBattleInterface(Player player, Boss boss)
    {
        Console.Clear();
        int boxWidth = 70;

        DrawTopBorder($"БОЙ: {boss.Name}", boxWidth);

        Console.Write(VERTICAL + " ");
        Console.Write($"Противник: {boss.Name}".PadRight(boxWidth - 4));
        Console.WriteLine(" " + VERTICAL);

        Console.Write(VERTICAL + " ");
        Console.Write("Здоровье босса: ".PadRight(boxWidth - 4 - 25));
        DrawHealthBar(boss.Health, boss.GetMaxHealth(), 22);
        Console.WriteLine(" " + VERTICAL);

        Console.Write(VERTICAL + " ");
        Console.Write("Ваше здоровье: ".PadRight(boxWidth - 4 - 25));
        DrawHealthBar(player.Health, player.MaxHealth, 22);
        Console.WriteLine(" " + VERTICAL);

        DrawSeparator(boxWidth);

        Console.Write(VERTICAL + " Части тела:".PadRight(boxWidth - 1));
        Console.WriteLine(VERTICAL);

        for (int i = 0; i < boss.BossBodyParts.Count; i++)
        {
            var part = boss.BossBodyParts[i];
            Console.Write(VERTICAL + " ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"[{i + 1}]");
            Console.ResetColor();
            Console.Write($" {part.Name} (x{part.DamageMultiplier})".PadRight(boxWidth - 10));
            Console.WriteLine(VERTICAL);
        }

        DrawBottomBorder(boxWidth);
    }

    public static void DrawEnemyEncounter(Enemy enemy, Player player)
    {
        Console.Clear();
        int boxWidth = 60;

        DrawTopBorder("ВСТРЕЧА С ВРАГОМ", boxWidth);

        Console.Write(VERTICAL + " ");
        Console.Write($"Враг: {enemy.Name}".PadRight(boxWidth - 4));
        Console.WriteLine(" " + VERTICAL);

        Console.Write(VERTICAL + " ");
        Console.Write("Здоровье врага: ".PadRight(boxWidth - 4 - 20));
        DrawHealthBar(enemy.Health, 100, 18);
        Console.WriteLine(" " + VERTICAL);

        Console.Write(VERTICAL + " ");
        Console.Write("Ваше здоровье: ".PadRight(boxWidth - 4 - 20));
        DrawHealthBar(player.Health, player.MaxHealth, 18);
        Console.WriteLine(" " + VERTICAL);

        DrawBottomBorder(boxWidth);
    }

    public static void ShowMessage(string title, string message, int width = 60)
    {
        Console.WriteLine();
        DrawTopBorder(title, width);
        
        var lines = message.Split('\n');
        for (int i = 0; i < Math.Max(lines.Length, 3); i++)
        {
            Console.Write(VERTICAL + " ");
            if (i < lines.Length)
            {
                Console.Write(lines[i].PadRight(width - 4));
            }
            else
            {
                Console.Write(new string(' ', width - 4));
            }
            Console.WriteLine(" " + VERTICAL);
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
            Console.Write(VERTICAL + " ");
            Console.ForegroundColor = color;
            Console.Write(label.PadRight(width - 4));
            Console.ResetColor();
            Console.WriteLine(" " + VERTICAL);
        }

        DrawBottomBorder(width);
        Console.WriteLine();
    }

    public static void DrawMainMenu()
    {
        Console.Clear();
        int width = 50;
        
        DrawTopBorder("╔═══════════════════╗", width);
        Console.Write(VERTICAL + " ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("   ДРЕВНЯЯ ТЬМА   ".PadRight(width - 4));
        Console.ResetColor();
        Console.WriteLine(" " + VERTICAL);
        
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
