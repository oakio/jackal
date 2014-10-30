namespace Jackal
{
    public class Direction
    {
        public Position From;
        public Position To;

        protected bool Equals(Direction other)
        {
            return Equals(From, other.From) && Equals(To, other.To);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Direction) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((From != null ? From.GetHashCode() : 0)*397) ^ (To != null ? To.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Direction left, Direction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Direction left, Direction right)
        {
            return !Equals(left, right);
        }
    }
}