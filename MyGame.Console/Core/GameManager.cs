using System;
using System.Threading;

public class GameManager
{
    private int MapHeight;
    private int MapWidth;
    public Difficulty CurrentDifficulty;
    private static GameManager? _instance;

    private GameManager()
    {
        MapHeight = 200;
        MapWidth = 200;
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

    public void Run()
    {
        Console.WriteLine("=== Welcome to the game! ===");
        Console.WriteLine($"Настройки загружены:");
        Console.WriteLine($"- Ширина карты: {MapWidth}");
        Console.WriteLine($"- Высота карты: {MapHeight}");
        Console.WriteLine($"- Сложность: {CurrentDifficulty}");
        Console.WriteLine("============================");
        Console.WriteLine("Press Esc to exit.");
        Console.WriteLine("Alexander is Gay");

        while (isRunning)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Escape)
                {
                    isRunning = false;
                }
            }

            if (isRunning)
            {
                Update();
                Data();
            }

            Thread.Sleep(16);
        }

        Console.WriteLine("Goodbye!");
    }

    private void Update()
    {
        
    }

    private void Data()
    {
        
    }
}