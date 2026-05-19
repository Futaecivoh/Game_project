using NUnit.Framework;
using MyGame.Console.Core.FSM;

namespace NUnit_Tests
{
    [TestFixture]
    public class GameFSMTests
    {
        [Test]
        public void GamePlayState_WhenEscapePressed_ShouldSwitchToPauseState()
        {
            var context = new GameContext();
            context.ChangeState(GamePlayState.Instance);

            context.HandleInput(ConsoleKey.Escape);

            Assert.That(context.CurrentState, Is.TypeOf<PauseState>());
        }

        [Test]
        public void PauseState_WhenEscapePressed_ShouldSwitchToGamePlayState()
        {
            var context = new GameContext();
            context.ChangeState(PauseState.Instance);

            context.HandleInput(ConsoleKey.Escape);

            Assert.That(context.CurrentState, Is.TypeOf<GamePlayState>());
        }

        [Test]
        public void GamePlayState_WhenHealthZero_ShouldSwitchToGameOverState()
        {
            var context = new GameContext(100); 
            context.ChangeState(GamePlayState.Instance);

            context.TakeDamage(100);

            Assert.That(context.CurrentState, Is.TypeOf<GameOverState>());
        }
    }
}