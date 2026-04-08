public class Boss : Creature
{
    public List<BossBodyPart> BossBodyParts { get; set; }

    public Boss(string name, int health, List<BossBodyPart> bodyParts)
    {
        Name = name;
        Health = health;
        BossBodyParts = bodyParts;
    }

    public override void Action()
    {
        Console.WriteLine($"{Name} готовится к атаке!");
    }
}