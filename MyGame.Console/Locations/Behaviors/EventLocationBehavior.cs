public class EventLocationBehavior : ILocationBehavior
{
    public void OnEnter(Location location, Player player)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" Вы находите древний алтарь! Ваше оружие начинает вибрировать...");
        Console.ResetColor();
        System.Threading.Thread.Sleep(1000);

        int roll = RNJesus.Range(GameBalance.RNJesusRangeMin, GameBalance.RNJesusRangeMax);

        if (roll == 1)
        {
            var oldWeapon = player.EquippedWeapon;
            player.EquippedWeapon = new FireEnchantment(player.EquippedWeapon, GameBalance.FireEnchantmentBonus);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("🔥 RNJesus благосклонен! Вы получили Зачарование Огня!");
            Console.WriteLine($"   Урон увеличен с {oldWeapon.GetDamage()} до {player.EquippedWeapon.GetDamage()}");
            Console.ResetColor();
        }
        else
        {
            var oldWeapon = player.EquippedWeapon;
            player.EquippedWeapon = new IceEnchantment(player.EquippedWeapon, GameBalance.IceEnchantmentBonus);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("❄️  RNJesus благосклонен! Вы получили Зачарование Льда!");
            Console.WriteLine($"   Урон увеличен с {oldWeapon.GetDamage()} до {player.EquippedWeapon.GetDamage()}");
            Console.ResetColor();
        }

        System.Threading.Thread.Sleep(1200);
    }
}
