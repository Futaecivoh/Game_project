abstract class Creature
{
    public string Name {get; set;}
    public int Hp {get; set;}

    public abstract void Move();
}