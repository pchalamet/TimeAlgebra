using System;

namespace TimeAlgebra
{
    public class Period<T>
    {
        internal Period(DateTime from, DateTime to, T value)
        {
            if (to <= from)
            {
                throw new ArgumentException("from must be strictly lower than to");
            }

            if (from.TimeOfDay != TimeSpan.Zero || to.TimeOfDay != TimeSpan.Zero)
            {
                throw new ArgumentException("from and to must be date not datetime");
            }

            From = from;
            To = to;
            Value = value;
        }

        public DateTime From { get; private set; }

        public DateTime To { get; private set; }

        public T Value { get; private set; }

        public static Period<T> Day(DateTime from, T value)
        {
            return new Period<T>(from, from.AddDays(1), value);
        }

        public static Period<T> Days(DateTime from, int nbDays, T value)
        {
            return new Period<T>(from, from.AddDays(nbDays), value);
        }

        public override string ToString()
        {
            return string.Format("[{0:d}, {1:d}[ := {2}", From, To, Value);
        }
    }
}
