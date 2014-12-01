using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jackal.RolloPlayer
{
	/// <summary>
	/// Вклад каждого оценщика в итоговую оценку
	/// </summary>
	public class MoveRateItem
	{
		public MoveRateItem(string rateActionName, double rate)
		{
			RateActionName = rateActionName;
			Rate = rate;
		}

		public string RateActionName;
		public double Rate;
		public bool ApplyToAll;
	}
}
