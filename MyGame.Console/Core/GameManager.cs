using MyGame.Console.Core.Commands;
using System;

public class GameManager
{
    public int MapHeight { get; private set; }
    public int MapWidth { get; private set; }
    public Difficulty CurrentDifficulty { get; private set; }
    
    public Player MainPlayer { get; private set; } = new Player();

    private static GameManager? _instance;
    
    private IRandomProvider _random = new RNJesusAdapter();
    private GameUIController? _uiController;

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
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
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
        _uiController = new GameUIController(this, map);

        _uiController.ShowGameStart();

        using var hud = new ConsoleHUD(MainPlayer);

        map.CurrentLocation = map.StartNode;
        map.CurrentLocation.Enter();

        while (isRunning)
        {
            _uiController.ShowLocationChoice();
            
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

                _uiController.ShowLocationEnter(map.CurrentLocation);

                if (map.CurrentLocation.Type == "Boss" && map.CurrentLocation.Boss != null)
                {
                    HandleBossFight(MainPlayer, map.CurrentLocation.Boss);
                }
            }
            else
            {
                _uiController.ShowInvalidChoice();
            }
        }

        _uiController.ShowGameOver(false);
    }

    private void HandleBossFight(Player player, Boss boss)
    {
        _uiController?.ShowBattleStart(boss);

        while (boss.Health > 0 && player.Health > 0)
        {
            _uiController?.ShowBattleMenu(boss);

            string? input = Console.ReadLine();

            if (int.TryParse(input, out int choice) && choice > 0 && choice <= boss.BossBodyParts.Count)
            {
                var targetPart = boss.BossBodyParts[choice - 1];

                bool hit = _random.Roll(GameBalance.HitChance);
                BossBodyPart actualPart;

                if (hit)
                {
                    actualPart = targetPart;
                }
                else
                {
                    int index = _random.Range(0, boss.BossBodyParts.Count - 1);
                    actualPart = boss.BossBodyParts[index];
                }
                
                int currentDamage = player.EquippedWeapon.GetDamage();
                int damageDealt = (int)(currentDamage * actualPart.DamageMultiplier);
                DealDamage(boss, actualPart.Name, currentDamage);

                _uiController?.ShowBattleUpdate(boss, hit, actualPart, damageDealt);
                
                if (!hit && boss.Health > 0)
                {
                    int bossDamage = _random.Range(GameBalance.BossMinDamage, GameBalance.BossMaxDamage);
                    player.TakeDamage(bossDamage);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"⚠️  Босс в ярости и атакует в ответ! Урон: {bossDamage}");
                    Console.ResetColor();
                    System.Threading.Thread.Sleep(800);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Неверный выбор.");
                Console.ResetColor();
            }
        }
       
        bool playerWon = player.Health > 0;
        _uiController?.ShowBattleResult(playerWon);
        isRunning = !playerWon;
    }

    public void DealDamage(Boss boss, string partName, int baseDamage)
        {
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