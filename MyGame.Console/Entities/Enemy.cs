public class Enemy : Creature
{
    public Enemy(string name, int hp)
    {
        Name = name;
        Hp = hp;
    }

    public override void Move()
    {
        Console.WriteLine($"[Враг] {Name} Враг типо");
    }

    public void Attack()
    {
        Console.WriteLine($"{Name} атакует!");
    }
}