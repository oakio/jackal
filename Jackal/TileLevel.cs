using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jackal
{
    public class TileLevel
    {
        [JsonProperty]
        public readonly TilePosition Position;

        [JsonProperty]
        public readonly HashSet<Pirate> Pirates = new HashSet<Pirate>();

        public TileLevel(TilePosition position)
        {
            Position = position;
        }

        public int? OccupationTeamId
        {
            get
            {
                if (Pirates != null)
                {
                    foreach (Pirate pirate in Pirates)
                    {
                        return pirate.TeamId;
                    }
                }
                return null;
            }
        }
        [JsonProperty]
        public int Coins;
    }
}