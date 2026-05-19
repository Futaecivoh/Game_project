public class BossLocationBehavior : ILocationBehavior
{
    public void OnEnter(Location location, Player player)
    {
        if (location.Boss == null)
            return;

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"⚡ Вы встретили босса: {location.Boss.Name} (HP: {location.Boss.Health})");
        Console.ResetColor();
    }
}
