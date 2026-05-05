namespace MyGame.Console.Core.Commands
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}