public class BossBodyPart
{
    public string Name { get; set; }
    public float DamageMultiplier { get; set; }

    public BodyPart(string name, float multiplier)
    {
        Name = name;
        DamageMultiplier = multiplier;
    }
}