namespace Jackal
{
    public interface IPlayer
    {
        Move OnMove(Board board, Move[] availableMoves);
    }
}