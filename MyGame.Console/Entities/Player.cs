public class Player : Creature
{
    public int Level { get; set; } = 1;
    public IWeapon EquippedWeapon { get; set; }

    public override void Action()
    {
        Console.WriteLine($"{Name} Душнит насчет D&D. Оружие: {EquippedWeapon.GetDescription()}"
        + $" (Урон: {EquippedWeapon.GetDamage()})");
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

    public Player()
    {
        Name = "Кирильченко";
        EquippedWeapon = new BasicSword(GameBalance.PlayerBaseDamage);
    }
}