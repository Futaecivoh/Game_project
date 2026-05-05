public sealed class ConsoleHUD : IDisposable
{
    private readonly Player _player;
    private readonly EventHandler<HealthChangedEventArgs> _handler;

    public ConsoleHUD(Player player)
    {
        _player = player;
        _handler = OnHealthChanged;
        _player.OnHealthChanged += _handler;
        PrintLine(_player.Health, _player.MaxHealth);
    }

    private void OnHealthChanged(object? sender, HealthChangedEventArgs e)
    {
        PrintLine(e.CurrentHealth, e.MaxHealth);
    }

    private static void PrintLine(int current, int max)
    {
        string bar = FormatHealthBar(current, max);
        Console.WriteLine($"[HP: {bar}] {current}/{max}");
    }

    public static string FormatHealthBar(int current, int max, int width = 10)
    {
        if (max <= 0)
            return new string('.', width);

        double ratio = (double)current / max;
        int filled = (int)Math.Round(ratio * width);
        filled = Math.Clamp(filled, 0, width);
        return new string('|', filled) + new string('.', width - filled);
    }

    public void Dispose()
    {
        _player.OnHealthChanged -= _handler;
    }
}
