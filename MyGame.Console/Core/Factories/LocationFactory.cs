public static class LocationFactory
{
    public static Location Create(int id, string name, string type)
    {
        return new Location
        {
            Id = id,
            Name = name,
            Type = type,
            IsOneTimeVisit = type == LocationType.OneTimeEvent,
            Behavior = CreateBehavior(type)
        };
    }

    public static ILocationBehavior CreateBehavior(string type) => type switch
    {
        LocationType.Start => new StartLocationBehavior(),
        LocationType.Enemy => new EnemyLocationBehavior(),
        LocationType.Event => new EventLocationBehavior(),
        LocationType.OneTimeEvent => new EventLocationBehavior(),
        LocationType.Boss => new BossLocationBehavior(),
        LocationType.Shop => new ShopLocationBehavior(),
        
        _ => new StartLocationBehavior()
    };
}