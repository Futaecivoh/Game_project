public class Player : Creature
{
    public int Level { get; set; } = 1;

    public override void Action()
    {
        Console.WriteLine($"{Name} защищается и ждёт команд");
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        Console.WriteLine($"Игрок получил {damage} урона. HP: {Health}");
    }

    public void GainXP(int xp)
    {
        Level += xp / 100;
        Console.WriteLine($"Игрок получил опыт! Уровень: {Level}");
    }
}