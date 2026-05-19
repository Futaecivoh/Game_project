using System.Text.Json;
public class SaveService
{
    private readonly string _savePath;

    public SaveService()
    {
        string folder = Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData);

        _savePath = Path.Combine(folder, "MyGame", "save.json");

        Directory.CreateDirectory(Path.GetDirectoryName(_savePath)!);
    }

    public void SaveGame(Player player)
    {
        var data = new SaveData
        {
            PlayerX = player.X,
            PlayerY = player.Y,
            Health = player.Health,
            Level = player.Level,
            InventoryItems = player.Inventory
        };

        string json = JsonSerializer.Serialize(
            data,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        File.WriteAllText(_savePath, json);
    }

    public void LoadGame(Player player)
    {
        if (!File.Exists(_savePath))
        {
            return;
        }

        string json = File.ReadAllText(_savePath);

        SaveData? data = JsonSerializer.Deserialize<SaveData>(json);

        if (data == null)
        {
            return;
        }

        player.X = data.PlayerX;
        player.Y = data.PlayerY;
        player.Health = data.Health;
        player.Level = data.Level;
        player.Inventory = data.InventoryItems;
    }
}