public class Enemy : Creature
{
    public int ExperienceReward { get; set; } = GameBalance.EnemyBaseXP;
    

    public Enemy()
    {
        _attackBehavior = new AggressiveBehavior();
    }

    public void SetBehavior(IAttackBehavior newBehavior)
    {
        _attackBehavior = newBehavior;
        Console.WriteLine($" {Name} меняет тактику!");
    }

    public void PerformBehavior(Player target)
    {
        _attackBehavior?.Execute(this, target);
    }

    public override void Action()
    {
        Console.WriteLine($"{Name} атакует игрока!");
    }
    private IAttackBehavior _attackBehavior;
}