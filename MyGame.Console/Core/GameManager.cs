using System;
using System.Threading;

public class GameManager
{
    public int MapHeight;
    public int MapWidth;
    public Difficulty CurrentDifficulty;
    private static GameManager? _instance;
    private IRandomProvider _random;

    private GameManager()
    {
        MapHeight = 200;
        MapWidth = 200;
        CurrentDifficulty = Difficulty.Medium;

        _random = new RNJesusAdapter();
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

    public void Run()
    {
        Console.WriteLine("=== Welcome to the Card Game! ===");
        
        MapBuilder builder = new MapBuilder();
        WorldMap map = builder
            .SetMapName("Акт 1: дремучий хуй")
            .AddLocation(1, "Старт", "Start")
            .AddLocation(2, "Засада врагов", "Enemy")
            .AddLocation(3, "Артефакты", "Event")
            .AddLocation(4, "Босс", "Boss")
            .Connect(1, 2)
            .Connect(1, 3)
            .Connect(2, 4)
            .Connect(3, 4)
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

        Player player = new Player 
        { 
            Name = "Hero", 
            Health = GameBalance.PlayerStartHealth 
        };

        Console.WriteLine($"Карта '{map.MapName}' готовченко!\n");

        if (map.StartNode == null)
        {
            Console.WriteLine("Ошибка: нет стартовой локации");
            return;
        }

        Location currentLocation = map.StartNode;
        currentLocation.Enter();

        while (isRunning)
        {
            Console.WriteLine("\nКуда отправимся дальше? (Выберите номер)");
            
            if (currentLocation.ConnectedLocations.Count == 0)
            {
                Console.WriteLine("Дальше пути нет. Конец Акта 1!");
                break;
            }

            for (int i = 0; i < currentLocation.ConnectedLocations.Count; i++)
            {
                var nextLoc = currentLocation.ConnectedLocations[i];
                Console.WriteLine($"[{i + 1}] -> {nextLoc.Name} ({nextLoc.Type})");
            }
            Console.WriteLine("[0] -> Выйти из игры");

            string? input = Console.ReadLine();
            
            if (input == "0")
            {
                isRunning = false;
                break;
            }

            if (int.TryParse(input, out int choice) &&
                choice > 0 &&
                choice <= currentLocation.ConnectedLocations.Count)
            {
                currentLocation = currentLocation.ConnectedLocations[choice - 1];
                currentLocation.Enter();

                if (currentLocation.Type == "Boss" && currentLocation.Boss != null)
                {
                    HandleBossFight(player, currentLocation.Boss);
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
        while (boss.Health > 0)
        {
            Console.WriteLine("\nКуда ударим?");
            
            for (int i = 0; i < boss.BossBodyParts.Count; i++)
            {
                var part = boss.BossBodyParts[i];
                Console.WriteLine($"[{i + 1}] {part.Name} (x{part.DamageMultiplier})");
            }

            string? input = Console.ReadLine();

            if (int.TryParse(input, out int choice) &&
                choice > 0 &&
                choice <= boss.BossBodyParts.Count)
            {
                var targetPart = boss.BossBodyParts[choice - 1];

                bool hit = _random.Roll(GameBalance.HitChance);

                BossBodyPart actualPart;

                if (hit)
                {
                    actualPart = targetPart;
                    Console.WriteLine("Попадание!");
                    Console.WriteLine($"HP босса: {boss.Health}");
                }
                else
                {
                    int index = _random.Range(0, boss.BossBodyParts.Count);
                    actualPart = boss.BossBodyParts[index];
                    Console.WriteLine($"Промах! Попали в {actualPart.Name}");
                    Console.WriteLine($"HP босса: {boss.Health}");
                    if (boss.Health > 0)
                    {
                        int bossDamage = _random.Range(
                            GameBalance.BossMinDamage, 
                            GameBalance.BossMaxDamage
                        );
                        Console.WriteLine("Босс атакует!");
                        player.TakeDamage(bossDamage);
                    }
                }

                int damage = (int)(GameBalance.PlayerBaseDamage * actualPart.DamageMultiplier);
                DealDamage(boss, damage);
            }
            else
            {
                Console.WriteLine("Неверный выбор.");
            }
        }

        Console.WriteLine($"Босс {boss.Name} побежден!");
    }

    public void DealDamage(Creature target, int damage)
    {
        target.Health -= damage;
    }

    private void Update()
    {
        
    }

    private void Data()
    {
        
    }
}