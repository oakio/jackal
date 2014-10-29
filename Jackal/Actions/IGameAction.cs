namespace Jackal.Actions
{
    interface IGameAction
    {
        GameActionResult Act(Game game);
    }

    internal enum GameActionResult
    {
        Live,
        Die
    }
}