using NUnit.Framework;
using System.Linq;

[TestFixture]
public class GameManagerTests
{
    private GameManager _gameManager;
    private Boss _boss;

    [SetUp]
    public void Setup()
    {
        _gameManager = GameManager.Instance;

        _boss = new BossBuilder()
            .SetName("TestBoss")
            .SetHealth(100)
            .AddBossBodyPart("Head", 2.0f)
            .AddBossBodyPart("Body", 1.0f)
            .Build();
    }

    [Test]
    public void DealDamage_ValidPart_ShouldReduceHealthCorrectly()
    {
        int baseDamage = 10;

        _gameManager.DealDamage(_boss, "Head", baseDamage);

        Assert.AreEqual(80, _boss.Health);
    }

    [Test]
    public void DealDamage_BodyPart_ShouldApplyNormalMultiplier()
    {
        int baseDamage = 10;

        _gameManager.DealDamage(_boss, "Body", baseDamage);

        Assert.AreEqual(90, _boss.Health);
    }

    [Test]
    public void DealDamage_Overkill_ShouldNotGoBelowZero()
    {
        int baseDamage = 100;

        _gameManager.DealDamage(_boss, "Head", baseDamage);

        Assert.AreEqual(0, _boss.Health);
    }

    [Test]
    public void DealDamage_InvalidPart_ShouldNotChangeHealth()
    {
        int baseDamage = 10;

        _gameManager.DealDamage(_boss, "Leg", baseDamage);

        Assert.AreEqual(100, _boss.Health);
    }

    [Test]
    public void DealDamage_ZeroDamage_ShouldNotChangeHealth()
    {
        int baseDamage = 0;

        _gameManager.DealDamage(_boss, "Head", baseDamage);

        Assert.AreEqual(100, _boss.Health);
    }

    [Test]
    public void DealDamage_NegativeDamage_ShouldNotHealBoss()
    {
        int baseDamage = -10;

        _gameManager.DealDamage(_boss, "Head", baseDamage);

        Assert.LessOrEqual(_boss.Health, 100);
    }
}