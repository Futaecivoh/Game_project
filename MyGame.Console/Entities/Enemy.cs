public class Enemy : Creature
{
    private int _health;
    
    public int MaxHealth { get; set; } = GameBalance.GoblinStartHealth;
    public int ExperienceReward { get; set; } = GameBalance.EnemyBaseXP;
    
    public override int Health
    {
        get => _health;
        set => _health = Math.Max(0, value);
    }

    public Enemy()
    {
        _attackBehavior = new AggressiveBehavior();
        _health = MaxHealth;
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