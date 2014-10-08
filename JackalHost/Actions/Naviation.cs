using Jackal;

namespace JackalHost.Actions
{
    class Naviation : GameAction
    {
        private readonly Ship _ship;
        private readonly Position _to;

        public Naviation(Board board, Ship ship, Position to) : base(board)
        {
            _ship = ship;
            _to = to;
        }

        public override void Act()
        {
            _ship.Position = _to;
            foreach (var pirate in _ship.Crew)
            {
                pirate.Position = _to;
            }
        }
    }
}