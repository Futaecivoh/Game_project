using System;

namespace MyGame.Console.Core.FSM
{
    public class GameOverState : IGameState
    {
        public static GameOverState Instance { get; } = new GameOverState();
        private GameOverState() { }

        public void HandleInput(GameContext context, ConsoleKey key)
        {
            if (key == ConsoleKey.Enter)
            {
                context.ChangeState(MenuState.Instance);
            }
        }

        public void Update(GameContext context) { /* Логика экрана смерти */ }
    }
}