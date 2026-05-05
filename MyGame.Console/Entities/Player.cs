public class Player : Creature
{
    private int _health;

    public int Level { get; set; } = 1;
    public IWeapon EquippedWeapon { get; set; }

    public int MaxHealth { get; }

    public event EventHandler<HealthChangedEventArgs>? OnHealthChanged;

    public override int Health
    {
        get => _health;
        set => ApplyHealth(value);
    }

    public Player()
    {
        Name = "Кирильченко";
        MaxHealth = GameBalance.PlayerStartHealth;
        _health = MaxHealth;
        EquippedWeapon = new BasicSword(GameBalance.PlayerBaseDamage);
    }

    public override void Action()
    {
        Console.WriteLine($"{Name} Душнит насчет D&D. Оружие: {EquippedWeapon.GetDescription()}"
        + $" (Урон: {EquippedWeapon.GetDamage()})");
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0)
            return;

        Health -= damage;
        Console.WriteLine($"Игрок получил {damage} урона.");
    }

    private void ApplyHealth(int newValue)
    {
        newValue = Math.Max(0, newValue);
        if (_health == newValue)
            return;

        _health = newValue;
        OnHealthChanged?.Invoke(this, new HealthChangedEventArgs(_health, MaxHealth));
    }

    public void GainXP(int xp)
    {
        Level += xp / GameBalance.XpPerLevel;
        Console.WriteLine($"Игрок получил опыт! Уровень: {Level}");
    }
}
