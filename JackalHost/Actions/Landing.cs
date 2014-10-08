using Jackal;

namespace JackalHost.Actions
{
    class Landing : GameAction
    {
        private readonly Pirate _pirate;
        private readonly Ship _ship;

        public Landing(Board board, Pirate pirate, Ship ship) : base(board)
        {
            _pirate = pirate;
            _ship = ship;
        }

        public override void Act()
        {
            _ship.Crew.Remove(_pirate);
        }
    }
}