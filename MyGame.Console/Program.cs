using System;
using System.Threading;

class Program
{
    static bool isRunning = true;

    static void Main(string[] args)
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

    static void Update()
    {
    
    }

    static void Data()
    {
    
    }
}