namespace Jackal
{
	public class Move
	{
		public Pirate Pirate;
		public Position From;
		public Position To;
		public bool WithCoins;

		public Move()
		{
		}

		public Move(Pirate pirate, Position to, bool withCoin)
		{
			Pirate = pirate;
			To = to;
			WithCoins = withCoin;

			// используется для отрисовки предыдущей позиции пирата
			From = new Position(pirate.Position.X, pirate.Position.Y);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public bool Equals(Move other)
		{
			return this.Pirate.Position == other.Pirate.Position
				   && this.To == other.To
				   && this.WithCoins == other.WithCoins;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Move && Equals((Move)obj);
		}

		public static bool operator ==(Move left, Move right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Move left, Move right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return string.Format("{0},{1},{2}", Pirate.Position, To, WithCoins ? "+": "o");
		}
	}
}