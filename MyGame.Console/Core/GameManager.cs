using MyGame.Console.Core.Commands;
using System;
using MyGame.Console.Core.SaveSystem;
using System.Text.Json;

public class GameManager
{
    public int MapHeight { get; private set; }
    public int MapWidth { get; private set; }
    public Difficulty CurrentDifficulty { get; private set; }
    
    public Player MainPlayer { get; private set; } = new Player();
    public CommandHistory GameHistory { get; private set; } = new CommandHistory();
    public SaveManager GameSaver { get; private set; } = new SaveManager();

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
    private bool? _actVictory;
    public bool IsInBossFight { get; private set; }

    public void Run()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        MapBuilder builder = new MapBuilder();
        WorldMap map = builder
            .SetMapName("Акт 1: Темный лес")
            .AddLocation(1, "Старт", LocationType.Start, 1)
            
            .AddLocation(2, "Засада врагов", LocationType.Enemy, 2)
            .AddLocation(3, "Лавка", LocationType.Shop, 2)
            
            .AddLocation(4, "Артефакты", LocationType.Event, 3)
            .AddLocation(5, "Кузница", LocationType.Forge, 3)
            .AddLocation(6, "Древний алтарь", LocationType.Forge, 3)
            .AddLocation(7, "Босс", LocationType.Boss, 4)
            
            .Connect(1, 2)
            .Connect(2, 4)
            .Connect(2, 5)
            .Connect(4, 7)
            .Connect(6, 7) 

            .Connect(1, 3) 
            .Connect(3, 5)
            .Connect(3, 6)
            .Connect(5, 7) 

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
            .FirstOrDefault(l => l.Type == LocationType.Boss);

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
        
        if(map.CurrentLocation != null)
        {    
        map.CurrentLocation.Enter();
        map.RecordVisit(map.CurrentLocation);
        }

        while (isRunning)
        {
            UIManager.ClearScreen();
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
            else if (input == "8") 
            {
                var data = new GameSaveData
                {
                    PlayerHealth = MainPlayer.Health,
                    PlayerLevel = MainPlayer.Level,
                    WeaponDamage = MainPlayer.EquippedWeapon.GetDamage(),
                    WeaponDescription = MainPlayer.EquippedWeapon.GetDescription(),
                    CurrentLocationId = map.CurrentLocation!.Id
                };
                GameSaver.SaveGame(data);
                continue;
            }
            else if (input == "7") 
            {
                var data = GameSaver.LoadGame();
                if (data != null)
                {
                    MainPlayer.Level = data.PlayerLevel;
                    MainPlayer.Health = data.PlayerHealth;
                    MainPlayer.EquippedWeapon = new RestoredWeapon(data.WeaponDamage, data.WeaponDescription);
                    
                    var loadedLocation = map.Locations.Values.FirstOrDefault(l => l.Id == data.CurrentLocationId);
                    if (loadedLocation != null)
                    {
                        map.CurrentLocation = loadedLocation;
                        
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n[Успех] Вы вернулись в локацию: {map.CurrentLocation.Name}");
                        Console.ResetColor();
                    }
                }
                continue;
            }
            else if (int.TryParse(input, out int choice))
            {
                var travelOptions = map.GetTravelOptions();
                if (choice > 0 && choice <= travelOptions.Count)
                {
                    var chosenLocation = travelOptions[choice - 1];
                    
                    bool isReturn = chosenLocation.Level <= map.CurrentLocation!.Level;

                    ICommand moveCmd = new MoveToNodeCommand(map, chosenLocation);
                    GameHistory.ExecuteCommand(moveCmd);

                    UIManager.ClearScreen();
                    _uiController.ShowLocationEnter(map.CurrentLocation!, isReturn);
                    map.CurrentLocation!.Enter();
                    map.RecordVisit(map.CurrentLocation);

                    if (map.CurrentLocation.Type == LocationType.Boss && map.CurrentLocation.Boss != null)
                    {
                        HandleBossFight(MainPlayer, map.CurrentLocation.Boss);
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("Нажмите любую клавишу, чтобы вернуться к карте...");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                }
                else
                {
                    _uiController.ShowInvalidChoice();
                    Thread.Sleep(1000);
                }
            }
            else
            {
                _uiController.ShowInvalidChoice();
                Thread.Sleep(1000);
            }
        }

        if (_actVictory.HasValue)
            _uiController.ShowGameOver(_actVictory.Value);
    }

    private void HandleBossFight(Player player, Boss boss)
    {
        IsInBossFight = true;
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
                    Console.WriteLine($"  Босс в ярости и атакует в ответ! Урон: {bossDamage}");
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
        IsInBossFight = false;
        _uiController?.ShowBattleResult(playerWon);

        if (playerWon)
        {
            _actVictory = true;
            isRunning = false;
        }
        else
        {
            _actVictory = false;
            isRunning = false;
        }
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