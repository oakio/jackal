using Jackal;

namespace JackalHost.Actions
{
    class Walk : IGameAction
    {
        private readonly Pirate _pirate;
        private readonly Position _to;

        public Walk(Pirate pirate, Position to)
        {
            _pirate = pirate;
            _to = to;
        }

        public void Act(Game game)
        {
            _pirate.Position = _to;
        }
    }
}