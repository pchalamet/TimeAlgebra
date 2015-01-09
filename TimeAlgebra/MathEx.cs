using System;

namespace TimeAlgebra
{
    public static class MathEx
    {
        public static T Min<T>(T a, T b) where T : IComparable
        {
            var cmp = a.CompareTo(b);
            return cmp <= 0
                ? a
                : b;
        }

        public static T Max<T>(T a, T b) where T : IComparable
        {
            var cmp = a.CompareTo(b);
            return cmp < 0
                ? b
                : a;
        }
    }
}
