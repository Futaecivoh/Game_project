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

    public bool IsImplemented => false;

    public bool SaveGame(Player player)
    {
        _ = player;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("⚠ Сохранение пока не реализовано (заглушка).");
        Console.ResetColor();
        return false;
    }

    public bool LoadGame(Player player)
    {
        _ = player;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("⚠ Загрузка пока не реализована (заглушка).");
        Console.ResetColor();
        return false;
    }

    public bool HasSaveFile()
    {
        return false;
    }

    public SaveData? ReadSaveDataStub()
    {
        return null;
    }

    public void WriteSaveDataStub(SaveData data)
    {
        _ = data;
    }
}
