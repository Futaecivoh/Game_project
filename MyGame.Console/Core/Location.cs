public class Location
{
    public int Id {get; set;}
    public string? Name {get; set;}
    public string? Type {get; set;}

    public List<Location> ConnectedLocations {get; set;} = new List<Location>();

    // 🔥 ДОБАВЬ ЭТО
    public Boss? Boss { get; set; }

    public void Enter()
    {
        Console.WriteLine($"\n Вы прибыли в локацию: {Name} ({Type})");

        if (Type == "Enemy")
        {
            Console.WriteLine("Заглушка");
        }

        if (Type == "Boss" && Boss != null)
        {
            Console.WriteLine($"Вы встретили босса: {Boss.Name} (HP: {Boss.Health})");
        }
    }
}