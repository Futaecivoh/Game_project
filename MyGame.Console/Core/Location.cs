public class Location
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? Type {get; set;}

    public List<Location> ConnectedLocations {get; set;} = new List<Location>();

    public Boss? Boss { get; set; }

    public void Enter()
    {
        Player player = GameManager.Instance.MainPlayer;

        if (Type == "Enemy")
        {
            Enemy goblin = new Enemy { Name = "Гоблин", Health = GameBalance.GoblinStartHealth };

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("🚨 Из кустов выпрыгивает враг!");
            Console.ResetColor();
            System.Threading.Thread.Sleep(800);

            goblin.PerformBehavior(player); 

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nВы наносите мощный ответный удар!");
            Console.ResetColor();
            goblin.Health = GameBalance.GoblinPostHitHealth;

            if (goblin.Health < GameBalance.GoblinFleeThreshold)
            {
                goblin.SetBehavior(new FleeBehavior());
            }

            goblin.PerformBehavior(player);
            System.Threading.Thread.Sleep(800);
        }

        if (Type == "Boss" && Boss != null)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"⚡ Вы встретили босса: {Boss.Name} (HP: {Boss.Health})");
            Console.ResetColor();
        }

        else if (Type == "Event")
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("✨ Вы находите древний алтарь! Ваше оружие начинает вибрировать...");
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
}