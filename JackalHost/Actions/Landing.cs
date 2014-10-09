using Jackal;

namespace JackalHost.Actions
{
    class Landing : IGameAction
    {
        private readonly Pirate _pirate;
        private readonly Ship _ship;

        public Landing(Pirate pirate, Ship ship)
        {
            _pirate = pirate;
            _ship = ship;
        }

        public void Act(Game game)
        {
            _ship.Crew.Remove(_pirate);
        }
    }
}