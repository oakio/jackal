using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jackal.RolloPlayer
{
	public class PosRate
	{
		public PosRate(Position pos)
		{
			Pos = pos;
		}

		public Position Pos;
		public double Rate = 1;
	}
}
