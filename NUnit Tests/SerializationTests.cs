using NUnit.Framework;
using System.Text.Json;
using MyGame.Console.Core.SaveSystem;

namespace NUnit_Tests
{
    [TestFixture]
    public class SerializationTests
    {
        [Test]
        public void GameSaveData_SerializeAndDeserialize_FieldsShouldMatch()
        {
            var originalData = new GameSaveData
            {
                PlayerHealth = 150,
                PlayerLevel = 10,
                WeaponDamage = 45,
                WeaponDescription = "Легендарный меч",
                CurrentLocationId = 5
            };

            string json = JsonSerializer.Serialize(originalData);
            var loadedData = JsonSerializer.Deserialize<GameSaveData>(json);

            Assert.That(loadedData, Is.Not.Null);
            Assert.That(loadedData.PlayerHealth, Is.EqualTo(150));
            Assert.That(loadedData.PlayerLevel, Is.EqualTo(10));
            Assert.That(loadedData.WeaponDamage, Is.EqualTo(45));
            Assert.That(loadedData.WeaponDescription, Is.EqualTo("Легендарный меч"));
            Assert.That(loadedData.CurrentLocationId, Is.EqualTo(5));
        }
    }
}