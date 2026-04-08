public class WorldMap
{
    public string ?MapName { get; set; }
    
    public Dictionary<int, Location> Locations { get; set; } = new Dictionary<int, Location>();
    
    public Location? StartNode { get; set; }
}