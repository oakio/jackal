namespace Jackal
{
    public interface IPlayer
    {
        int OnMove(Board board, Move[] availableMoves);
    }
}