namespace MyGame.Console.Core.FSM
{
    public class GameContext
    {
        public IGameState CurrentState { get; private set; }
        public int PlayerHP { get; private set; }

        public GameContext(int initialHP = 100)
        {
            PlayerHP = initialHP;
            CurrentState = MenuState.Instance; 
        }

        public void ChangeState(IGameState newState)
        {
            CurrentState = newState;
        }

        public void HandleInput(ConsoleKey key)
        {
            CurrentState.HandleInput(this, key);
        }

        public void Update()
        {
            CurrentState.Update(this);
        }

        public void TakeDamage(int damage)
        {
            PlayerHP -= damage;
            Update(); 
        }
    }
}