using MyGame.Console.Core.Commands;

namespace NUnit_Tests
{
    [TestFixture]
    public class CommandTests
    {
        [Test]
        public void MoveCommand_Undo_ShouldReturnToPreviousLocation()
        {
            var history = new CommandHistory();
            var map = new WorldMap();
            
            var startNode = LocationFactory.Create(1, "Развилка", LocationType.Event);
            var shopNode = LocationFactory.Create(2, "Магазин артефактов", LocationType.Shop);

            startNode.ConnectedLocations.Add(shopNode);
            map.CurrentLocation = startNode;

            var moveToShop = new MoveToNodeCommand(map, shopNode);

            history.ExecuteCommand(moveToShop);

            Assert.That(map.CurrentLocation, Is.EqualTo(shopNode));

            history.UndoLastCommand();

            Assert.That(map.CurrentLocation, Is.EqualTo(startNode));
        }
    }
}