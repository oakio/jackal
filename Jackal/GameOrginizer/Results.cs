using System;
using System.Collections.Generic;
using System.Linq;

namespace Jackal.GameOrginizer
{
    public class Results
    {
        Dictionary<string, ResultRec> table = new Dictionary<string, ResultRec>();
        public volatile int MapId;
        public volatile int PermutationId;
        public volatile int GamesCount;

        public string GetState()
        {
            String rez = "";
            lock (table)
            {
                foreach (var pair in table.OrderBy(x => x.Value.AveragePosition))
                {
                    rez += string.Format("{0}: средняя позиция {1:N3}, вероятности {2:N3} {3:N3} {4:N3} {5:N3}\r\n",
                        pair.Key,
                        pair.Value.AveragePosition,
                        pair.Value.Position1 / pair.Value.Total,
                        pair.Value.Position2 / pair.Value.Total,
                        pair.Value.Position3 / pair.Value.Total,
                        pair.Value.Position4 / pair.Value.Total);
                }
            }
            return rez;
        }

        public void AddGameResult(string clientId, double[] positionsCount)
        {
            if (positionsCount == null || positionsCount.Length != 4)
                throw new ArgumentException();
            lock (table)
            {
                ResultRec rec;
                if (table.TryGetValue(clientId, out rec) == false)
                {
                    rec = new ResultRec();
                    table[clientId] = rec;
                }
                rec.Position1 += positionsCount[0];
                rec.Position2 += positionsCount[1];
                rec.Position3 += positionsCount[2];
                rec.Position4 += positionsCount[3];
            }
        }
    }
}