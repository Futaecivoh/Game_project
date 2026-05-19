using System;
using System.Collections.Generic;

public class GameUIController
{
    private readonly GameManager _gameManager;
    private readonly WorldMap _map;
    private Player Player => _gameManager.MainPlayer;

    public GameUIController(GameManager gameManager, WorldMap map)
    {
        _gameManager = gameManager;
        _map = map;
    }

    public void ShowWelcomeScreen()
    {
        UIManager.DrawMainMenu();
    }

    public void ShowGameStart()
    {
        UIManager.ClearScreen();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔═══════════════════════════════════════╗");
        Console.WriteLine("║     Добро пожаловать в игру!          ║");
        Console.WriteLine("║           ТЁМНЫЕ ВЕКА                 ║");
        Console.WriteLine("╚═══════════════════════════════════════╝");
        Console.ResetColor();
        
        Console.WriteLine();
        Console.WriteLine($"Ваше имя: {Player.Name}");
        Console.WriteLine($"Уровень: {Player.Level}");
        Console.WriteLine($"Максимальное здоровье: {Player.MaxHealth}");
        
        var weapon = Player.EquippedWeapon;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\nВы начинаете путь. В руках у вас:");
        Console.WriteLine($"  {weapon.GetDescription()} (Урон: {weapon.GetDamage()})");
        Console.ResetColor();
        
        Console.WriteLine($"\nКарта '{_map.MapName}' готова!");
        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    public void ShowLocationChoice()
    {
        UIManager.DrawMap(_map);
        Console.WriteLine();
        Console.WriteLine("[0] - Выйти из игры");
        Console.WriteLine("[9] - Отмена последнего шага");
        Console.Write("\nВаш выбор: ");
    }

    public void ShowInvalidChoice()
    {
        UIManager.ShowMessage("ОШИБКА", "Неверный выбор, попробуйте еще раз.", 50);
    }

    public void ShowLocationEnter(Location location, bool isReturn = false)
    {
        Console.WriteLine();
        if (isReturn)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($">>> Вы возвращаетесь: {location.Name}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($">>> Вы прибыли: {location.Name}");
        }
        Console.ResetColor();
    }

    public void ShowEnemyEncounter(Enemy enemy)
    {
        UIManager.DrawEnemyEncounter(enemy, Player);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nИз кустов выпрыгивает {enemy.Name}!");
        Console.ResetColor();
    }

    public void ShowBattleStart(Boss boss)
    {
        UIManager.DrawBattleInterface(Player, boss);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n⚔️  ВЫ ВОШЛИ В ЛОГОВО БОССА: {boss.Name}!");
        Console.ResetColor();
        System.Threading.Thread.Sleep(1000);
    }

    public void ShowBattleUpdate(Boss boss, bool hit, BossBodyPart targetPart, int damageDealt)
    {
        UIManager.DrawBattleInterface(Player, boss);
        
        if (hit)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✓ Попадание!");
            Console.WriteLine($"  Вы ударили {targetPart.Name}!");
            Console.WriteLine($"  Урон: {damageDealt}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n✗ Промах!");
            Console.ResetColor();
        }

        System.Threading.Thread.Sleep(500);
    }

    public void ShowBattleMenu(Boss boss)
    {
        Console.WriteLine("\n┌─ ВЫБЕРИТЕ ЦЕЛЬ ─┐");
        for (int i = 0; i < boss.BossBodyParts.Count; i++)
        {
            var part = boss.BossBodyParts[i];
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"[{i + 1}]");
            Console.ResetColor();
            Console.WriteLine($"    {part.Name} (x{part.DamageMultiplier})");
        }
        Console.Write("\nВаш выбор: ");
    }

    public void ShowBattleResult(bool playerWon)
    {
        if (playerWon)
        {
            UIManager.ShowMessage("ПОБЕДА", "Вы одолели босса!\n\nПолучена награда!");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░░");
        }
        else
        {
            UIManager.ShowMessage("ПОРАЖЕНИЕ", "Вы были побеждены...\n\nКонец игры.");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("░░░░░░░░░░░░░░░░░░░░");
        }
        Console.ResetColor();
    }

    public void ShowEventEncounter(string title, string message)
    {
        UIManager.ShowMessage(title, message, 70);
    }

    public void ShowWeaponUpgrade(IWeapon oldWeapon, IWeapon newWeapon)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("⭐ УЛУЧШЕНИЕ ОРУЖИЯ!");
        Console.WriteLine($"  Было: {oldWeapon.GetDescription()} (Урон: {oldWeapon.GetDamage()})");
        Console.WriteLine($"  Стало: {newWeapon.GetDescription()} (Урон: {newWeapon.GetDamage()})");
        Console.ResetColor();
        System.Threading.Thread.Sleep(1500);
    }

    public void ShowGameOver(bool playerWon)
    {
        Console.Clear();
        if (playerWon)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║           ПОЗДРАВЛЯЕМ!                 ║");
            Console.WriteLine("║      Вы победили в этом акте!          ║");
            Console.WriteLine("║      Да будет слава вашему имени!      ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║           ИГРА ОКОНЧЕНА                ║");
            Console.WriteLine("║      Вы не пережили это испытание      ║");
            Console.WriteLine("║           Попробуйте позже             ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
        }
        Console.ResetColor();
    }

    public void ShowPlayerStats()
    {
        UIManager.DrawPlayerStats(Player);
        Console.WriteLine("Нажмите любую клавишу для возврата...");
        Console.ReadKey();
    }
}
