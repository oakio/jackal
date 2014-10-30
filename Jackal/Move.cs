using System;

namespace Jackal
{
    public class Move : Direction
	{
		public Pirate Pirate;
		public bool WithCoins;
	    public bool WithRespawn;

		public Move()
		{
		}

        protected bool Equals(Move other)
        {
            return base.Equals(other) && WithCoins.Equals(other.WithCoins) && WithRespawn.Equals(other.WithRespawn);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Move) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode*397) ^ WithCoins.GetHashCode();
                hashCode = (hashCode*397) ^ WithRespawn.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Move left, Move right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Move left, Move right)
        {
            return !Equals(left, right);
        }

        public Move(Pirate pirate, Position to, bool withCoin)
		{
			Pirate = pirate;
			To = to;
			WithCoins = withCoin;

			// используется для отрисовки предыдущей позиции пирата
			From = new Position(pirate.Position.X, pirate.Position.Y);
		}

		public override string ToString()
		{
		    char code = 'o';
		    if (WithCoins)
		        code = '+';
            else if (WithRespawn)
                code = '❤';
		    return string.Format("{0},{1},{2}", From, To, code);
		}
	}
}