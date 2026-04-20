public class AggressiveBehavior : IAttackBehavior
{
    public void Execute(Enemy attacker, Player target)
    {
        int damage = 15; 
        Console.WriteLine($"\n {attacker.Name} агрессивно бросается в атаку и наносит {damage} урона!");
        target.TakeDamage(damage);
    }
}