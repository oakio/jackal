using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jackal
{
    public class Tile
    {
        [JsonProperty]
        public readonly Position Position;
        [JsonProperty]
        public readonly TileType Type;

        [JsonProperty]
        public readonly int ArrowsCode;
        [JsonProperty]
        public readonly int SpinningCount;

        public int Coins
        {
            get
            {
                return Levels[0].Coins;
            }
        }

        public int? OccupationTeamId
        {
            get
            {
                return Levels[0].OccupationTeamId;
            }
        }

        public HashSet<Pirate> Pirates
        {
            get
            {
                return Levels[0].Pirates;
            }
        }

        /// <summary>
        /// Уровни клетки (0 - обычный уровень/уровень выхода с клетки)
        /// </summary>
        public readonly List<TileLevel> Levels = new List<TileLevel>();

        public Tile()
        {
        }

        public Tile(TileParams tileParams)
        {
            Position = tileParams.Position;
            Type = tileParams.Type;
            int levelsCount = (tileParams.Type == TileType.Spinning) ? tileParams.SpinningCount : 1;
            for (int level = 0; level < levelsCount; level++)
            {
                var tileLevel = new TileLevel(new TilePosition(tileParams.Position, level));
                Levels.Add(tileLevel);
            }
            ArrowsCode = tileParams.ArrowsCode;
            SpinningCount = tileParams.SpinningCount;
        }
    }
}