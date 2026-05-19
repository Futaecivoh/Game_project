public class SaveData
{
    public string Difficulty { get; set; } = string.Empty;
    public string PlayerName { get; set; } = string.Empty;
    public int PlayerHealth { get; set; }
    public int PlayerLevel { get; set; }
    public WeaponSaveData Weapon { get; set; } = new();
    public int CurrentLocationId { get; set; }
    public int BossHealth { get; set; }
}
