using MyGame.Console.Core;

namespace MyGame.Console.Core.Commands
{
    public class MoveToNodeCommand : ICommand
    {
        private readonly WorldMap _map;
        private readonly Location _targetNode;
        private Location? _previousNode;
        public MoveToNodeCommand(WorldMap map, Location targetNode)
        {
            _map = map;
            _targetNode = targetNode;
        }                                                                                                                                   

        public void Execute()
        {
            _previousNode = _map.CurrentLocation;
            _map.PreviousLocation = _previousNode;
            _map.CurrentLocation = _targetNode;
        }

        public void Undo()
        {
            if (_previousNode != null)
            {
                _map.PreviousLocation = _map.CurrentLocation;
                _map.CurrentLocation = _previousNode;
            }
        }
    }
}