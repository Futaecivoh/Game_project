public class Enemy : Creature
{
    public int ExperienceReward { get; set; } = GameBalance.EnemyBaseXP;

    public override void Action()
    {
        Console.WriteLine($"{Name} атакует игрока!");
    }
}

