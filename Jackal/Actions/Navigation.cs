namespace Jackal.Actions
{
    class Navigation : IGameAction
    {
        private readonly Ship _ship;
        private readonly Position _to;

        public Navigation(Ship ship, Position to)
        {
            _ship = ship;
            _to = to;
        }

        public void Act(Game game)
        {
            _ship.Position = _to;
            foreach (var pirate in _ship.Crew)
            {
                pirate.Position = _to;
            }
        }
    }
}