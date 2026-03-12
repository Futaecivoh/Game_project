using System;
using System.Threading;

public class GameManager
{
    
    private static GameManager _instance;

    private GameManager() { }

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

    
    private bool isRunning = true;

    public void Run()
    {
        Console.WriteLine("Welcome to the game! Press Esc to exit.");

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