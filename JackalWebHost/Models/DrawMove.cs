using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JackalWebHost.Models
{
    public class DrawMove
    {
        public LevelPosition From;
        public LevelPosition To;
        public bool WithCoin;
        public bool WithRespawn; 
    }
}