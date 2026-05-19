public class Boss : Creature
{
    public int MaxHealth { get; set; }
    public List<BossBodyPart> BossBodyParts { get; set; }

    public Boss(string name, int health, List<BossBodyPart> bodyParts)
    {
        Name = name;
        Health = health;
        MaxHealth = health;
        BossBodyParts = bodyParts;
    }

    public int GetMaxHealth() => MaxHealth;

    public override void Action()
    {
        Console.WriteLine($"{Name} готовится к атаке!");
    }
}