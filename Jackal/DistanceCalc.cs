using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jackal
{
	/// <summary>
	/// Считальщик расстояний
	/// </summary>
	public class DistanceCalc
	{
		private readonly Board _board;
		private readonly double _unknownSteps;
		private Dictionary<string, Wave> _waves = new Dictionary<string, Wave>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="board"></param>
		/// <param name="unknownSteps">Оценка хождения по закрытой клетке</param>
		public DistanceCalc(Board board, double unknownSteps = 1)
		{
			_board = board;
			_unknownSteps = unknownSteps;
		}

		/// <summary>
		/// Расстояние от from до to
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="teamId"></param>
		/// <param name="withGold"></param>
		/// <returns></returns>
		public double? Distance(Position from, Position to, int teamId, bool withGold = false)
		{
			if (from == to) return 0;

			var wave = BuildWave(from, teamId, withGold);
			return  wave.Distance(to);
		}

		private Wave BuildWave(Position from, int teamId, bool withGold = false)
		{
			var w = new Wave{From = from, TeamId = teamId, WithGold = withGold};
			Wave res;

			if (_waves.TryGetValue(w.ToString(), out res))
				return res;

			var prevPos = new List<CalcPosition> { new CalcPosition(0, from) };

			do
			{
				var newPos = new List<CalcPosition>();
				foreach (var pos in prevPos)
				{
					var position = pos;
					//TODO: Вызывать правильную функцию GetAllAvaliableMoves с результатами которые отдаются плеерам для хода
					//TODO: если нужны только ходы с золотом - из результатов убирать ходы без золота
					newPos.AddRange(_board.GetAllAvaliableMoves(teamId, position.Position, new List<CheckedPosition>())
									.Select(p => new CalcPosition(position.Distance + position.SelfDistance(_board, _unknownSteps), p.Target))
									.ToList());	
				}

				newPos = w.AddStep(newPos);

				if (!newPos.Any()) break;
				prevPos = newPos;
			} while (true);

			_waves[w.ToString()] = w;

			return w;
		}

		/// <summary>
		/// Позиция с расстоянием до нее от старта
		/// </summary>
		public class CalcPosition
		{
			public CalcPosition(double distance, Position pos)
			{
				Distance = distance;
				Position = pos;
			}

			public double Distance;
			public Position Position;

			public double SelfDistance(Board board, double unknownSteps)
			{
				var tile = board.Map[Position];
				switch (tile.Type)
				{
					case TileType.Canibal:
					case TileType.Croc:
					case TileType.Water:
					case TileType.Trap:
						return 10000;
					case TileType.RumBarrel:
						return 2;
					case TileType.Unknown:
						return unknownSteps;
					case TileType.Spinning:
						return tile.SpinningCount;
					default:
						return 1;
				}
			}
		}

		public class Wave
		{
			public Position From;
			public int TeamId;
			public bool WithGold;
			private readonly Dictionary<Position, double> _calculated = new Dictionary<Position, double>();

			public override string ToString()
			{
				return string.Format("{0}:{1}:{2}", From, TeamId, WithGold );
			}

			/// <summary>
			/// Добавить рассчитанные значения следующего шага
			/// </summary>
			/// <param name="step"></param>
			/// <returns></returns>
			public List<CalcPosition> AddStep(List<CalcPosition> step)
			{
				var res = new List<CalcPosition>();
				foreach (var pos in step)
				{
					if (pos.Distance > 1000)
						continue;

					if (_calculated.ContainsKey(pos.Position))
					{
						if (pos.Distance < _calculated[pos.Position])
						{
							_calculated[pos.Position] = pos.Distance;
							res.Add(pos);
						}
					}
					else
					{
						_calculated.Add(pos.Position, pos.Distance);
						res.Add(pos);
					}
				}

				return res;
			}

			private readonly List<CheckedPosition> _checkedPositions = new List<CheckedPosition>();
			public List<CheckedPosition> CheckedPositions
			{
				get
				{
					return _checkedPositions;
				}
			}

			/// <summary>
			/// Расстояние до
			/// </summary>
			/// <param name="to"></param>
			/// <returns></returns>
			public double? Distance(Position to)
			{
				double res;
				if (_calculated.TryGetValue(to, out res))
				{
					return res > 1000 ? (double?)null : res;
				}

				return null;
			}
		}
	}

	
}
