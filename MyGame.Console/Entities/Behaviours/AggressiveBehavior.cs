using System.Security.Cryptography.X509Certificates;

public class AggressiveBehavior : IAttackBehavior
{
    public const int damage = 15;

    public void Execute(Enemy attacker, Player target)
    {
        Console.WriteLine($"\n {attacker.Name} агрессивно бросается в атаку и наносит {damage} урона!");
        target.TakeDamage(damage);
    }
}