public interface IWeapon
{
    int GetDamage();
    string GetDescription();
}

public class BasicSword : IWeapon
{
    private int _baseDamage;

    public BasicSword(int baseDamage)
    {
        _baseDamage = baseDamage;
    }

    public int GetDamage() => _baseDamage;
    public string GetDescription() => "Стартовый меч";
}

public abstract class WeaponDecorator : IWeapon
{
    protected IWeapon _weapon;

    public WeaponDecorator(IWeapon weapon)
    {
        _weapon = weapon;
    }

    public virtual int GetDamage() => _weapon.GetDamage();
    public virtual string GetDescription() => _weapon.GetDescription();
}

public class FireEnchantment : WeaponDecorator
{
    private int _bonusDamage;

    public FireEnchantment(IWeapon weapon, int bonusDamage) : base(weapon)
    {
        _bonusDamage = bonusDamage;
    }

    public override int GetDamage() => base.GetDamage() + _bonusDamage;
    public override string GetDescription() => base.GetDescription() + " (+Огонь)";
}

public class IceEnchantment : WeaponDecorator
{
    private int _bonusDamage;

    public IceEnchantment(IWeapon weapon, int bonusDamage) : base(weapon)
    {
        _bonusDamage = bonusDamage;
    }

    public override int GetDamage() => base.GetDamage() + _bonusDamage;
    public override string GetDescription() => base.GetDescription() + " (+Лед)";
}