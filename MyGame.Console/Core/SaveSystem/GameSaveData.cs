namespace MyGame.Console.Core.SaveSystem
{
    public class GameSaveData
    {
        public int PlayerHealth { get; set; }
        public int PlayerLevel { get; set; }
        
        public string WeaponDescription { get; set; } = "";
        public int WeaponDamage { get; set; }
        
        public int CurrentLocationId { get; set; }
    }
}