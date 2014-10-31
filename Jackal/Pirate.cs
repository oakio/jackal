using Newtonsoft.Json;

namespace Jackal
{
    public class Pirate
    {
        [JsonProperty]
        public readonly int TeamId;
        [JsonProperty]
        public TilePosition Position;
        [JsonProperty]
        public bool IsDrunk;

        internal int? DrunkSinceTurnNo;
        
        public Pirate(int teamId, TilePosition position)
        {
            TeamId = teamId;
            Position = position;
        }
    }
}