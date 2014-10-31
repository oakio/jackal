namespace Jackal.Actions
{
    public interface IGameAction
    {
        GameActionResult Act(Game game);
    }

    public enum GameActionResult
    {
        Live,
        Die
    }
}