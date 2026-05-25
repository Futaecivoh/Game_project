public class ForgeLocationBehavior : ILocationBehavior
{
    public void OnEnter(Location location, Player player)
    {
        UIManager.ShowMessage("КУЗНИЦА", "Здесь горячо и пахнет углем.\n(Заглушка: Здесь будет механика улучшения оружия)");
    }
}