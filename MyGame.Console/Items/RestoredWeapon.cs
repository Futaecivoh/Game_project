public class RestoredWeapon : IWeapon
{
    private int _damage;
    private string _description;

    public RestoredWeapon(int damage, string desc)
    {
        _damage = damage;
        _description = desc;
    }

    public int GetDamage() => _damage;
    public string GetDescription() => _description;
}