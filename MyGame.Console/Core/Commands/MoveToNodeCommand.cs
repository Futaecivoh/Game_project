namespace MyGame.Console.Core.Commands
{
    public class MoveToNodeCommand : ICommand
    {
        private readonly WorldMap _map;
        private readonly Location _targetNode;
        private Location? _previousNode;
        private Location? _oldPreviousLocation;

        public MoveToNodeCommand(WorldMap map, Location targetNode)
        {
            _map = map;
            _targetNode = targetNode;
        }

        public void Execute()
        {
            _previousNode = _map.CurrentLocation;
            _oldPreviousLocation = _map.PreviousLocation;
            _map.PreviousLocation = _previousNode;
            _map.CurrentLocation = _targetNode;
        }

        public void Undo()
        {
            if (_previousNode == null)
                return;

            _map.CurrentLocation = _previousNode;
            _map.PreviousLocation = _oldPreviousLocation;
            _map.UnrecordVisit(_targetNode);
        }
    }
}