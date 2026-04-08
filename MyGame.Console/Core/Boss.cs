public class Boss
{
    public string Name { get; set; }
    public int Health { get; set; }
    public List<BossBodyPart> BossBodyParts { get; set; }

    public Boss(string name, int health, List<BossBodyPart> bodyParts)
    {
        Name = name;
        Health = health;
        BossBodyParts = bodyParts;
    }
}