﻿using System;

namespace TimeAlgebra
{
    public static class PeriodizationExtensions
    {
        public static Periodization<T> Append<T>(this Periodization<T> @this, Period<T> period)
        {
            var list = PeriodizationAlgorithm.Union(@this.Periods, period);
            return new Periodization<T>(list);
        }

        public static Periodization<T3> Intersect<T1, T2, T3>(this Periodization<T1> @this, Period<T2> period, Func<T1, T2, T3> builder)
        {
            var list = PeriodizationAlgorithm.Intersect(@this.Periods, period, builder);
            return new Periodization<T3>(list);
        }
    }
}