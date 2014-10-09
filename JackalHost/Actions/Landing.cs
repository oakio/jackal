using System;
using Jackal;

namespace JackalHost.Actions
{
    class Landing : IGameAction
    {
        private readonly Pirate _pirate;
        private readonly Ship _ship;

        public Landing(Pirate pirate, Ship ship)
        {
            _pirate = pirate;
            _ship = ship;
        }

        public void Act(Game game)
        {
            Position shipPosition = _ship.Position;
            Position landingPosition;
            if (shipPosition.X == 0)
            {
                landingPosition = new Position(shipPosition.X + 1, shipPosition.Y);
            }
            else if(shipPosition.X == (Board.Size-1))
            {
                landingPosition = new Position(shipPosition.X - 1, shipPosition.Y);
            }
            else if(shipPosition.Y == 0)
            {
                landingPosition = new Position(shipPosition.X, shipPosition.Y + 1);
            }
            else if (shipPosition.Y == (Board.Size - 1))
            {
                landingPosition = new Position(shipPosition.X, shipPosition.Y - 1);
            }
            else
            {
                throw new NotSupportedException();
            }

            _ship.Crew.Remove(_pirate);
            _pirate.Position = landingPosition;

            var tile = game.Board.Map[landingPosition.X, landingPosition.Y];
            tile.OccupationTeamId = _pirate.TeamId;
            tile.Pirates.Add(_pirate);
        }
    }
}