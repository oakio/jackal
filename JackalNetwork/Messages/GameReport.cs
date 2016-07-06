using System;
using Jackal;

namespace JackalNetwork
{
  

    public class GameReport : NetworkMessage
    {
        public Guid GameId;
        public bool IsGameOver;
        public GameState State;
        public string[] GamersId;
        public int[] GamersStanding;
    }
}