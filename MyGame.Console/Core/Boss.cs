public class Boss
{
    public string Name { get; set; }
    public int Health { get; set; }
    public List<BodyPart> BodyParts { get; set; }

    public Boss(string name, int health, List<BodyPart> bodyParts)
    {
        Name = name;
        Health = health;
        BodyParts = bodyParts;
    }
}