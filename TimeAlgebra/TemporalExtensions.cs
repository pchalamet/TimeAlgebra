using System;
using System.Linq;

namespace TimeAlgebra
{
    public static class TemporalExtensions
    {
        public static Temporal<T> Append<T>(this Temporal<T> @this, Period<T> period)
        {
            var list = TemporalAlgorithm.Union(@this.Periods, period);
            return new Temporal<T>(list);
        }

        public static Temporal<T> Append<T>(this Temporal<T> @this, Temporal<T> temporal)
        {
            var unions = temporal.Periods.Aggregate(@this, (p, e) => p.Append(e));
            return unions;
        }

        public static Temporal<T3> Intersect<T1, T2, T3>(this Temporal<T1> @this, Period<T2> period, Func<T1, T2, T3> builder)
        {
            var list = TemporalAlgorithm.Intersect(@this.Periods, period, builder);
            return new Temporal<T3>(list);
        }

        public static Temporal<T3> Intersect<T1, T2, T3>(this Temporal<T1> @this, Temporal<T2> temporal, Func<T1, T2, T3> builder)
        {
            var inters = from x in temporal.Periods
                         let inter = @this.Intersect(x, builder)
                         select inter;
            var res = inters.Aggregate(Temporal<T3>.Empty, (p, e) => p.Append(e));
            return res;
        }

        public static Temporal<T3> Combine<T1, T2, T3>(this Temporal<T1> @this, Period<T2> period, Func<T1, T2, T3> builder)
        {
            var list = TemporalAlgorithm.Combine(@this.Periods, period, builder);
            return new Temporal<T3>(list);
        }

        public static Temporal<T3> Combine<T1, T2, T3>(this Temporal<T1> @this, Temporal<T2> temporal, Func<T1, T2, T3> builder)
        {
            var inters = from x in temporal.Periods
                         let inter = @this.Combine(x, builder)
                         select inter;
            var res = inters.Aggregate(Temporal<T3>.Empty, (p, e) => p.Append(e));
            return res;
        }
    }
}
