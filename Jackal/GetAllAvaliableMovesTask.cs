using System.Collections.Generic;

namespace Jackal
{
    public class GetAllAvaliableMovesTask
    {
        public int TeamId;
        public List<CheckedPosition> alreadyCheckedList = new List<CheckedPosition>();
        public TilePosition FirstSource;
        public TilePosition PreviosSource;
        public bool AddCoinMoving = true;
    }
}