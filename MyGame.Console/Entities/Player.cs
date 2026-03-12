class Player : Creature
{
    public Player(string name, int hp)
    {
        Name = name;
        Hp = hp;
    }
    public override void Move()
    {
        Console.WriteLine($"[Игрок] {Name} Смело делает шаг вперед");
    }
}