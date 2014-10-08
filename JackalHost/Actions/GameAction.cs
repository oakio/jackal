using Jackal;

namespace JackalHost.Actions
{
    public abstract class GameAction : IGameAction
    {
        protected readonly Board Board;

        protected GameAction(Board board)
        {
            Board = board;
        }

        public abstract void Act();
    }
}