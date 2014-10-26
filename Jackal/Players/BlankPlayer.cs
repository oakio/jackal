namespace Jackal.Players
{
    public abstract class BlankPlayer : IPlayer
    {
        private SmartPlayer player;

        public virtual void OnNewGame()
        {
            player = new SmartPlayer();
            player.OnNewGame();
        }

        public virtual int OnMove(GameState gameState)
        {
            return player.OnMove(gameState);
        }
    }
}