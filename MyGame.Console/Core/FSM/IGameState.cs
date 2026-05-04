namespace MyGame.Console.Core.FSM
{
    public interface IGameState
    {
        void HandleInput(GameContext context, ConsoleKey key);
        void Update(GameContext context);
    }
}