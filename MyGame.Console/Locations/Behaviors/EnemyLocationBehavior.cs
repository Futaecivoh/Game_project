public class EnemyLocationBehavior : ILocationBehavior
{
    public void OnEnter(Location location, Player player)
    {
        Enemy goblin = new Enemy { Name = "Гоблин", Health = GameBalance.GoblinStartHealth };

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("🚨 Из кустов выпрыгивает враг!");
        Console.ResetColor();
        System.Threading.Thread.Sleep(800);

        goblin.PerformBehavior(player);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nВы наносите мощный ответный удар!");
        Console.ResetColor();
        goblin.Health = GameBalance.GoblinPostHitHealth;

        if (goblin.Health < GameBalance.GoblinFleeThreshold)
            goblin.SetBehavior(new FleeBehavior());

        goblin.PerformBehavior(player);
        System.Threading.Thread.Sleep(800);
    }
}
