using System;

public class AltarLocationBehavior : ILocationBehavior 
{
    public void OnEnter(Location location, Player player)
    {
        UIManager.ShowMessage("ДРЕВНИЙ АЛТАРЬ", "Вы преклоняете колени перед полуразрушенным алтарем...\nТеплый свет окутывает вас, исцеляя раны!");
        
        int healAmount = 30;
        player.Health = Math.Min(player.MaxHealth, player.Health + healAmount);
        
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Ваше здоровье восстановлено! Текущее здоровье: {player.Health}/{player.MaxHealth}"); //
        Console.ResetColor();
    }
}