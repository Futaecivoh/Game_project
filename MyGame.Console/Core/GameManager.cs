using MyGame.Console.Core.Commands;

public class GameManager
{
    public int MapHeight { get; private set; }
    public int MapWidth { get; private set; }
    public Difficulty CurrentDifficulty { get; private set; }
    
    public Player MainPlayer { get; private set; } = new Player();

    private static GameManager? _instance;
    
    private IRandomProvider _random = new RNJesusAdapter();

    private GameManager()
    {
        MapHeight = GameBalance.MapHeight;
        MapWidth = GameBalance.MapWidth;
        CurrentDifficulty = Difficulty.Medium;
    }

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    public enum Difficulty
    {
        Hard, Medium, Easy
    }
    
    private bool isRunning = true;
    public CommandHistory GameHistory { get; private set; } = new CommandHistory();

    public void Run()
    {
        Console.WriteLine("=== Welcome to the Card Game! ===");
        
        MapBuilder builder = new MapBuilder();
        WorldMap map = builder
            .SetMapName("Акт 1: Темный лес")
            .AddLocation(1, "Старт", "Start")
            .AddLocation(2, "Засада врагов", "Enemy")
            .AddLocation(3, "Артефакты", "Event")
            .AddLocation(4, "Босс", "Boss")
            .AddLocation(5, "Торговец", "Shop")
            .Connect(1, 2)
            .Connect(1, 3)
            .Connect(2, 4)
            .Connect(3, 4)
            .Connect(1, 5)
            .SetStartLocation(1)
            .Build();

        Boss boss = new BossBuilder()
            .SetName("Dragon")
            .SetHealth(500)
            .AddBossBodyPart("Head", 2.0f)
            .AddBossBodyPart("Body", 1.0f)
            .AddBossBodyPart("Tail", 1.5f)
            .Build();

        var bossLocation = map.Locations.Values
            .FirstOrDefault(l => l.Type == "Boss");

        if (bossLocation == null)
        {
            Console.WriteLine("Ошибка: локация босса не найдена");
            return;
        }

        bossLocation.Boss = boss;

        
        
        MainPlayer = new Player();

        using var hud = new ConsoleHUD(MainPlayer);

        var weapon = MainPlayer.EquippedWeapon;
        Console.WriteLine($"Вы начинаете путь. В руках у вас: {weapon.GetDescription()} " +
                          $"(Урон: {weapon.GetDamage()})");

        Console.WriteLine($"Карта '{map.MapName}' готовченко!\n");

        if (map.StartNode == null)
        {
            Console.WriteLine("Ошибка: нет стартовой локации");
            return;
        }

        map.CurrentLocation = map.StartNode;
        map.CurrentLocation.Enter();

        bool hudDemoShown = false;

        while (isRunning)
        {
            if (!hudDemoShown)
            {
                hudDemoShown = true;
                Console.WriteLine("Жизненные показатели героя подключены! Краткая сводка:");
                Console.WriteLine($"Имя: {MainPlayer.Name}");
                Console.WriteLine($"Уровень: {MainPlayer.Level}");
                Console.WriteLine($"Здоровье: {MainPlayer.Health}");
                Console.WriteLine($"Оружие: {MainPlayer.EquippedWeapon.GetDescription()}");
                Console.WriteLine($"Урон: {MainPlayer.EquippedWeapon.GetDamage()}");
            }

            Console.WriteLine("\nКуда отправимся дальше? (Выберите номер)");
            
            if (map.CurrentLocation.ConnectedLocations.Count == 0)
            {
                Console.WriteLine("Дальше пути нет. Конец Акта 1!");
            }

            for (int i = 0; i < map.CurrentLocation.ConnectedLocations.Count; i++)
            {
                var nextLoc = map.CurrentLocation.ConnectedLocations[i];
                Console.WriteLine($"[{i + 1}] -> {nextLoc.Name} ({nextLoc.Type})");
            }
            
            Console.WriteLine("[0] -> Выйти из игры");
            Console.WriteLine("[9] -> Вернуться назад (Отмена шага)");

            string? input = Console.ReadLine();
            
            if (input == "0")
            {
                isRunning = false;
                break;
            }

           else if (input == "9")
            {
                GameHistory.UndoLastCommand();
                continue;
            }
            else if (int.TryParse(input, out int choice) &&
                choice > 0 &&
                choice <= map.CurrentLocation.ConnectedLocations.Count)
            {
                var chosenLocation = map.CurrentLocation.ConnectedLocations[choice - 1];

                ICommand moveCmd = new MoveToNodeCommand(map, chosenLocation);
                GameHistory.ExecuteCommand(moveCmd);

                map.CurrentLocation.Enter();

                if (map.CurrentLocation.Type == "Boss" && map.CurrentLocation.Boss != null)
                {
                    HandleBossFight(MainPlayer, map.CurrentLocation.Boss);
                }
            }
            else
            {
                Console.WriteLine("Неверный выбор, попробуйте еще раз.");
            }
        }

        Console.WriteLine("Goodbye!");
    }

    private void HandleBossFight(Player player, Boss boss)
    {
        Console.WriteLine($"\nВЫ ВОШЛИ В ЛОГОВО БОССА: {boss.Name}!");
        Console.WriteLine($"У босса {boss.Health} ХП. Ваше ХП: {player.Health}");
        
        while (boss.Health > 0 && player.Health > 0)
        {
            Console.WriteLine("\nКуда ударим?");
            
            for (int i = 0; i < boss.BossBodyParts.Count; i++)
            {
                var part = boss.BossBodyParts[i];
                Console.WriteLine($"[{i + 1}] {part.Name} (x{part.DamageMultiplier})");
            }

            string? input = Console.ReadLine();

            if (int.TryParse(input, out int choice) && choice > 0 && choice <= boss.BossBodyParts.Count)
            {
                var targetPart = boss.BossBodyParts[choice - 1];

                
                bool hit = _random.Roll(GameBalance.HitChance);

                BossBodyPart actualPart;

                if (hit)
                {
                    actualPart = targetPart;
                    Console.WriteLine("\n🎯 Попадание!");
                }
                else
                {
                   
                    int index = _random.Range(0, boss.BossBodyParts.Count - 1);
                    actualPart = boss.BossBodyParts[index];
                    Console.WriteLine($"\n💨 Промах! Оружие соскользнуло и попало в: {actualPart.Name}");
                }
                
                int currentDamage = player.EquippedWeapon.GetDamage();
                
                DealDamage(boss, actualPart.Name, currentDamage);

                Console.WriteLine($"Вы ударили оружием '{player.EquippedWeapon.GetDescription()}'!");
                Console.WriteLine($"Осталось HP босса: {boss.Health}");
                
                if (!hit && boss.Health > 0)
                {
                    int bossDamage = _random.Range(GameBalance.BossMinDamage, GameBalance.BossMaxDamage);
                    Console.WriteLine($"\n⚠️ Босс в ярости от вашей ошибки и атакует в ответ!");
                    player.TakeDamage(bossDamage);
                }
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
            }
        }
       
        if (player.Health <= 0)
        {
            Console.WriteLine("\n Вы погибли в бою с боссом... Игра окончена.");
        }
        else
        {
            Console.WriteLine($"\n Босс {boss.Name} побежден!");
        }
        
        isRunning = false;
    }

    public void DealDamage(Boss boss, string partName, int baseDamage)
    {
        if (baseDamage < 0)
        {
        baseDamage = 0; 
        }
        
        var part = boss.BossBodyParts.FirstOrDefault(p => p.Name == partName);

        if (part != null)
        {
            int finalDamage = (int)(baseDamage * part.DamageMultiplier);
            boss.Health -= finalDamage;
        }
        if (boss.Health < 0)
        {
            boss.Health = 0;
        }
    }
}