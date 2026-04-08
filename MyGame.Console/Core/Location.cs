public class Location
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? Type {get; set;}

    public List<Location> ConnectedLocations {get; set;} = new List<Location>();

    public Boss? Boss { get; set; }

    public void Enter()
    {
        Console.WriteLine($"\n Вы прибыли в локацию: {Name} ({Type})");

        if (Type == "Enemy")
        {
            Console.WriteLine("Но никто не пришёл на фан-встречу...");
        }

        if (Type == "Boss" && Boss != null)
        {
            Console.WriteLine($"Вы встретили босса: {Boss.Name} (HP: {Boss.Health})");
        }

        else if (Type == "Event")
        {
            Console.WriteLine("Вы находите древний алтарь! Ваше оружие начинает вибрировать...");
            
            Player player = GameManager.Instance.MainPlayer;
            
            
            int roll = RNJesus.Range(1, 2); 
            
            if (roll == 1)
            {
                player.EquippedWeapon = new FireEnchantment(player.EquippedWeapon, 20);
                Console.WriteLine("RNJesus благосклонен! Вы получили Зачарование Огня (+20 урона)!");
            }
            else
            {
                player.EquippedWeapon = new IceEnchantment(player.EquippedWeapon, 15);
                Console.WriteLine("RNJesus благосклонен! Вы получили Зачарование Льда (+15 урона)!");
            }
            
            Console.WriteLine($"Теперь ваше оружие: {player.EquippedWeapon.GetDescription()}");
            Console.WriteLine($"Ваш новый базовый урон: {player.EquippedWeapon.GetDamage()}");
        }
    }
}