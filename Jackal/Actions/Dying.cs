namespace Jackal.Actions
{
    class Dying : IGameAction
    {
        private readonly Pirate _pirate;

        public Dying(Pirate pirate)
        {
            _pirate = pirate;
        }

        public GameActionResult Act(Game game)
        {
            game.KillPirate(_pirate);

            return GameActionResult.Die;
        }
    }
}