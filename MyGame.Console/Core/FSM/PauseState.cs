namespace MyGame.Console.Core.FSM
{
    public class PauseState : IGameState
    {
        public static PauseState Instance { get; } = new PauseState();
        private PauseState() { }

        public void HandleInput(GameContext context, ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                context.ChangeState(GamePlayState.Instance);
            }
        }

        public void Update(GameContext context) { }
    }
}