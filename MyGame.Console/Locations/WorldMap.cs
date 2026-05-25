public class WorldMap
{
    public string? MapName { get; set; }

    public Dictionary<int, Location> Locations { get; set; } = new Dictionary<int, Location>();

    public Location? StartNode { get; set; }
    public Location? CurrentLocation { get; set; }
    public Location? PreviousLocation { get; set; }

    public HashSet<int> VisitedLocationIds { get; } = new HashSet<int>();

    public bool HasVisited(Location location) => VisitedLocationIds.Contains(location.Id);

    public bool CanTravelTo(Location location)
    {
        if (location.IsOneTimeVisit && HasVisited(location))
            return false;
        return true;
    }

    public void RecordVisit(Location location)
    {
        VisitedLocationIds.Add(location.Id);
    }

    public void UnrecordVisit(Location location)
    {
        VisitedLocationIds.Remove(location.Id);
    }

    public List<Location> GetTravelOptions()
    {
        var options = new List<Location>();
        if (CurrentLocation == null)
            return options;

        foreach (var loc in Locations.Values)
        {
            if (loc.ConnectedLocations.Any(c => c.Id == CurrentLocation.Id))
            {
                if (CanTravelTo(loc) && !options.Any(o => o.Id == loc.Id))
                    options.Add(loc);
            }
        }

        foreach (var next in CurrentLocation.ConnectedLocations)
        {
            if (CanTravelTo(next) && !options.Any(o => o.Id == next.Id))
                options.Add(next);
        }

        return options.OrderBy(o => o.Id).ToList();
    }

    public bool IsReturnPath(Location destination) =>
        PreviousLocation != null && destination.Id == PreviousLocation.Id;
}
