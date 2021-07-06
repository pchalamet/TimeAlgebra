using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeAlgebra
{
    public class Temporal<T>
    {
        public static Temporal<T> Empty = new Temporal<T>();

        private readonly IEnumerable<Period<T>> _periods;

        // used to create an empty temporal
        private Temporal()
            : this(Enumerable.Empty<Period<T>>())
        {
        }

        // used to add a new element in a temporal
        internal Temporal(IEnumerable<Period<T>> periods)
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
