using Jackal;

namespace JackalHost.Actions
{
    class Shipping : GameAction
    {
        private readonly Pirate _pirate;
        private readonly Ship _ship;

        public Shipping(Board board, Pirate pirate, Ship ship) : base(board)
        {
            _pirate = pirate;
            _ship = ship;
        }

        public override void Act()
        {
            _pirate.Position = _ship.Position;
            _ship.Crew.Add(_pirate);
            _ship.Coins += _pirate.Coins;
            _pirate.Coins = 0;
        }
    }
}