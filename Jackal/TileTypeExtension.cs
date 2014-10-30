namespace Jackal
{
    public static class TileTypeExtension
    {
        public static bool IsGold(this TileType source)
        {
            return source == TileType.Chest1 || source == TileType.Chest2 || source == TileType.Chest3 || source == TileType.Chest4 || source == TileType.Chest5;
        }

        /// <summary>
        /// Клетка требует немедленного движения по попаданию на неё?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool RequreImmediateMove(this TileType type)
        {
            return (type == TileType.Arrow
                    || type == TileType.Horse
                    || type == TileType.Balloon
                    || type == TileType.Ice
                    || type == TileType.Croc);
        }

        public static bool IsFort(this TileType source)
        {
            return source == TileType.Fort || source == TileType.RespawnFort;
        }
    }
}