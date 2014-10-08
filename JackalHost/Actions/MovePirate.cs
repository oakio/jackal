using Jackal;

namespace JackalHost.Actions
{
    public class MovePirate : GameAction
    {
        private readonly Pirate _pirate;
        private readonly Position _to;

        public MovePirate(Board board, Pirate pirate, Position to) : base(board)
        {
            _pirate = pirate;
            _to = to;
        }

        public override void Act()
        {
            _pirate.Position = _to;
        }
    }
}