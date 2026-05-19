public class ShopLocationBehavior : ILocationBehavior
{
    public void OnEnter(Location location, Player player)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("🛒 Торговец кивает вам — покупки скоро будут доступны.");
        Console.ResetColor();
        System.Threading.Thread.Sleep(600);
    }
}
