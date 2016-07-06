﻿namespace Jackal
{
    public class CheckedPosition
    {
        public TilePosition Position;
        public Position IncomeDelta;

        public CheckedPosition(TilePosition position, Position incomeDelta = null)
        {
            Position = position;
            IncomeDelta = incomeDelta;
        }
    }
}