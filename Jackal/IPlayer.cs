namespace Jackal
{
    public interface IPlayer
    {
        void OnNewGame();
        int OnMove(GameState gameState);
    }
}