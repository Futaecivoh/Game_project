public abstract class Creature
{
    public string ?Name { get; set; }
    public virtual int Health { get; set; }

    public abstract void Action();
}