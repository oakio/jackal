using System;

namespace Jackal
{
    public class Position
    {
        public  int X;
        public int Y;

        public Position()
        {
        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Position other)
        {
            if (other == null) return false;

            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Position && Equals((Position) obj);
        }

        public static bool operator ==(Position left, Position right)
        {
            if ((Object)left == (Object)right)
            {
                return true;
            }

            if ((Object)left == null || (Object)right == null)
            {
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            if ((Object)left == (Object)right)
            {
                return false;
            }

            if ((Object)left == null || (Object)right == null)
            {
                return true;
            }

            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X*397) ^ Y;
            }
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", X, Y);
        }
    }
}