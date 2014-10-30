namespace Jackal
{
    public class CheckedPosition
    {
        public Position Position;
        public Position IncomeDelta;

        public CheckedPosition(Position position, Position incomeDelta = null)
        {
            Position = position;
            IncomeDelta = incomeDelta;
        }
    }
}