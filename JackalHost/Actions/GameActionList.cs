using System.Collections.Generic;

namespace JackalHost.Actions
{
    public class GameActionList : IGameAction
    {
        private readonly List<IGameAction> _actions;

        public GameActionList(params IGameAction[] actions)
        {
            _actions = new List<IGameAction>(actions);
        }

        public void Act()
        {
            foreach (var action in _actions)
            {
                action.Act();
            }
        }

        public static GameActionList Create(params IGameAction[] actions)
        {
            return new GameActionList(actions);
        }

        public GameActionList AddIf(IGameAction action, bool condition)
        {
            if (condition)
            {
                _actions.Add(action);
            }
            return this;
        }
    }
}