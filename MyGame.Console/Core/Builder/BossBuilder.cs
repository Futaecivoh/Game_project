public class BossBuilder
{
    private string _name;
    private int _health;
    private List<BossBodyPart> _parts = new List<BossBodyPart>();

    public BossBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public BossBuilder SetHealth(int health)
    {
        _health = health;
        return this;
    }

    public BossBuilder AddBossBodyPart(string name, float multiplier)
    {
        _parts.Add(new BossBodyPart(name, multiplier));
        return this;
    }

    public Boss Build()
    {
        return new Boss(_name, _health, _parts);
    }
}