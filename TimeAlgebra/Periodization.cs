using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeAlgebra
{
    public class Periodization<T>
    {
        public static Periodization<T> Empty = new Periodization<T>();

        private readonly IEnumerable<Period<T>> _periods;

        // used to create an empty periodization
        private Periodization()
            : this(Enumerable.Empty<Period<T>>())
        {
        }

        // used to add a new element in a periodization
        internal Periodization(IEnumerable<Period<T>> periods)
        {
            _periods = periods;
        }

        public IEnumerable<Period<T>> Periods
        {
            get { return _periods; }
        }

        public bool TryGetValue(DateTime date, out T value)
        {
            foreach (var period in Periods)
            {
                if (period.From <= date && date < period.To)
                {
                    value = period.Value;
                    return true;
                }
            }

            value = default(T);
            return false;
        }

        public override string ToString()
        {
            return Periods.Aggregate(new StringBuilder(), (sb, p) => sb.AppendLine(p.ToString())).ToString();
        }
    }
}
