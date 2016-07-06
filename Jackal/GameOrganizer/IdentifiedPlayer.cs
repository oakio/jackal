﻿namespace Jackal.GameOrganizer
{
    public class IdentifiedPlayer
    {
        public readonly IPlayer Player;
        public readonly string Id;

        public IdentifiedPlayer(IPlayer player, string id)
        {
            Player = player;
            Id = id;
        }
    }
}