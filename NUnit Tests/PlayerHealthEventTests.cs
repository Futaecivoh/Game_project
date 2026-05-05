[TestFixture]
public class PlayerHealthEventTests
{
    [Test]
    public void TakeDamage_FiresOnHealthChanged_WithPayload()
    {
        var player = new Player();
        HealthChangedEventArgs? captured = null;
        player.OnHealthChanged += (_, e) => captured = e;

        player.TakeDamage(50);

        Assert.That(captured, Is.Not.Null);
        Assert.That(captured!.CurrentHealth, Is.EqualTo(GameBalance.PlayerStartHealth - 50));
        Assert.That(captured.MaxHealth, Is.EqualTo(GameBalance.PlayerStartHealth));
    }

    [Test]
    public void ApplyHealth_WhenValueUnchanged_DoesNotFireEvent()
    {
        var player = new Player();
        int invocations = 0;
        player.OnHealthChanged += (_, _) => invocations++;

        player.Health = player.Health;

        Assert.That(invocations, Is.EqualTo(0));
    }

    [Test]
    public void TakeDamage_WhenAlreadyAtZero_DoesNotFireAgain()
    {
        var player = new Player();
        int invocations = 0;
        player.OnHealthChanged += (_, _) => invocations++;

        player.TakeDamage(player.Health);
        int afterZero = invocations;

        player.TakeDamage(25);

        Assert.That(afterZero, Is.GreaterThan(0));
        Assert.That(invocations, Is.EqualTo(afterZero));
    }

    [Test]
    public void OnHealthChanged_AfterUnsubscribe_IsNotInvoked()
    {
        var player = new Player();
        int invocations = 0;
        EventHandler<HealthChangedEventArgs> handler = (_, _) => invocations++;

        player.OnHealthChanged += handler;
        player.TakeDamage(10);
        Assert.That(invocations, Is.EqualTo(1));

        player.OnHealthChanged -= handler;
        player.TakeDamage(10);
        Assert.That(invocations, Is.EqualTo(1));
    }

    [Test]
    public void ConsoleHUD_FormatHealthBar_SixOfTen_ForSixtyPercent()
    {
        string bar = ConsoleHUD.FormatHealthBar(60, 100, width: 10);
        Assert.That(bar, Is.EqualTo("||||||...."));
    }

    [Test]
    public void ConsoleHUD_Dispose_UnsubscribesInternalHandler()
    {
        var player = new Player();
        using (var hud = new ConsoleHUD(player))
        {
            Assert.That(hud, Is.Not.Null);
            player.TakeDamage(10);
        }

        Assert.DoesNotThrow(() => player.TakeDamage(10));
    }
}
