using Jackal;

namespace JackalHost.Actions
{
    class Shipping : IGameAction
    {
        private readonly Pirate _pirate;
        private readonly Ship _ship;

        public Shipping(Pirate pirate, Ship ship)
        {
            _pirate = pirate;
            _ship = ship;
        }

        public void Act(Game game)
        {
            _pirate.Position = _ship.Position;
            _ship.Crew.Add(_pirate);
            _ship.Coins += _pirate.Coins;
            _pirate.Coins = 0;
        }
    }
}