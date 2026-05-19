public class MapBuilder
{
    private WorldMap _map = new WorldMap();

    public MapBuilder SetMapName(string name)
    {
        _map.MapName = name;
        return this;
    }

    public MapBuilder AddLocation(int id, string name, string type)
    {
        _map.Locations[id] = LocationFactory.Create(id, name, type);
        return this;
    }

    public MapBuilder Connect(int fromId, int toId)
    {
        if (_map.Locations.ContainsKey(fromId) && _map.Locations.ContainsKey(toId))
        {
            _map.Locations[fromId].ConnectedLocations.Add(_map.Locations[toId]);
        }
        return this;
    }

    public MapBuilder SetStartLocation(int id)
    {
        if (_map.Locations.ContainsKey(id))
        {
            _map.StartNode = _map.Locations[id];
        }
        return this;
    }

    public WorldMap Build()
    {
        WorldMap result = _map;
        _map = new WorldMap(); 
        return result;
    }
}