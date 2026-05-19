using System.Collections.Generic;

namespace MyGame.Console.Core.Commands
{
    public class CommandHistory
    {
        private Stack<ICommand> _history = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _history.Push(command);
        }

        public void UndoLastCommand()
        {
            if (_history.Count > 0)
            {
                ICommand lastCommand = _history.Pop();
                lastCommand.Undo();
                System.Console.WriteLine("Действие отменено (Undo).");
            }
            else
            {
                System.Console.WriteLine("Нет действий для отмены!");
            }
        }
    }
}