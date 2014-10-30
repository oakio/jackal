namespace Jackal
{
    public class PossibleMove
    {
        public PossibleMoveType Type;
        public Position Target;

        public PossibleMove(Position target, PossibleMoveType type = PossibleMoveType.Usual)
        {
            Target = target;
            Type = type;
        }
    }
}