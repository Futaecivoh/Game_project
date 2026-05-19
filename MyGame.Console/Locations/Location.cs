public class Location
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public bool IsOneTimeVisit { get; set; }

    public ILocationBehavior Behavior { get; set; } = new StartLocationBehavior();

    public List<Location> ConnectedLocations { get; set; } = new List<Location>();

    public Boss? Boss { get; set; }

    public void Enter()
    {
        Player player = GameManager.Instance.MainPlayer;
        Behavior.OnEnter(this, player);
    }
}
