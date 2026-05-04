namespace MyGame.Console.Core.FSM
{
    public class MenuState : IGameState
    {
        public static MenuState Instance { get; } = new MenuState();
        private MenuState() { } 

        public void HandleInput(GameContext context, ConsoleKey key)
        {
            if (key == ConsoleKey.Enter)
            {
                context.ChangeState(GamePlayState.Instance);
            }
        }

        public void Update(GameContext context) { /* Логика меню */ }
    }
}