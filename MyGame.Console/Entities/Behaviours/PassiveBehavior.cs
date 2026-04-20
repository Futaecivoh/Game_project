public class PassiveBehavior : IAttackBehavior
{
    public void Execute(Enemy attacker, Player target)
    {
        Console.WriteLine($"\n🛡️ {attacker.Name} стоит в защитной стойке, злобно смотрит, но ничего не делает.");
    }
}