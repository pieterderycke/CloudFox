using System;

namespace CloudFox.Util
{
    public class Tuple<T1, T2> : IEquatable<Tuple<T1, T2>>
    {
        private T1 value1;
        private T2 value2;

        public Tuple(T1 value1, T2 value2)
        {
            this.value1 = value1;
            this.value2 = value2;
        }

        public override bool Equals(object obj)
        {
            Tuple<T1, T2> other = obj as Tuple<T1, T2>;
            return Equals(other);
        }

        public bool Equals(Tuple<T1, T2> other)
        {
            return other != null && this.value1 != null && this.value2 != null &&
                this.value1.Equals(other.value1) && this.value2.Equals(other.value2);
        }

        public override int GetHashCode()
        {
            return this.value1.GetHashCode() ^ this.value2.GetHashCode();
        }
    }
}
