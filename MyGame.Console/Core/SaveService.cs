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

    }

    public void LoadGame(Player player)
    {

    }
}