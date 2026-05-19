using System;

namespace MyGame.Console.Core.FSM
{
    public class GamePlayState : IGameState
    {
        public static GamePlayState Instance { get; } = new GamePlayState();
        private GamePlayState() { }

        public void HandleInput(GameContext context, ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                context.ChangeState(PauseState.Instance);
            }
        }

        public void Update(GameContext context)
        {
            if (context.PlayerHP <= 0)
            {
                context.ChangeState(GameOverState.Instance);
            }
        }
    }
}