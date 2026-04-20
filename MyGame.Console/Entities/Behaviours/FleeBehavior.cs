public class FleeBehavior : IAttackBehavior
{
    public void Execute(Enemy attacker, Player target)
    {
        Console.WriteLine($"\n {attacker.Name} в панике бросает оружие и убегает с поля боя!");
        attacker.Health = 0; 
    }
}