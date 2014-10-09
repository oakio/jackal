using System.Collections.Generic;
using Jackal;

namespace JackalHost.Actions
{
    class GameActionList : IGameAction
    {
        private readonly List<IGameAction> _actions;

        public GameActionList(params IGameAction[] actions)
        {
            _actions = new List<IGameAction>(actions);
        }

        public void Act(Game game)
        {
            foreach (var action in _actions)
            {
                action.Act(game);
            }
        }

        public static GameActionList Create(params IGameAction[] actions)
        {
            return new GameActionList(actions);
        }
    }
}