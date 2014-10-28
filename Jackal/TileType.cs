namespace Jackal
{
	public enum TileType
	{
		Unknown,
		Water,
		Grass,
		Chest1,
		Chest2,
		Chest3,
		Chest4,
		Chest5,
        Fort,
        RespawnFort
	}

	public static class TileTypeExtension
	{
		public static bool IsGold(this TileType source)
		{
			return source == TileType.Chest1 || source == TileType.Chest2 || source == TileType.Chest3 || source == TileType.Chest4 || source == TileType.Chest5;
		}


        public static bool IsFort(this TileType source)
        {
            return source == TileType.Fort || source == TileType.RespawnFort;
        }
	}
}