using System;
using System.Collections.Generic;

namespace TimeAlgebra
{
    internal class PeriodizationAlgorithm
    {
        // prerequisite: enumerator is from a valid periodization
        public static IEnumerable<Period<T3>> Intersect<T1, T2, T3>(IEnumerable<Period<T1>> enumerable, Period<T2> atom, Func<T1, T2, T3> builder)
        {
            var result = new List<Period<T3>>();
            var enumerator = enumerable.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                // skip current
                // atom:              [------[
                // current:     [-----[
                if (current.To <= atom.From)
                {
                    continue;
                }
                // ==> atom.From < current.To

                // no more intersection
                // atom:       [------[
                // current:           [---------------[
                if (atom.To <= current.From)
                {
                    break;
                }
                // ==> current.From < atom.To

                // intersection
                // atom:        [------------[
                // current:         [---------------[
                // current:        [---[
                // current:   [---[
                // current:  [-----------------[
                if (current.From < atom.To)
                {
                    var value = builder(current.Value, atom.Value);
                    var from = MathEx.Max(current.From, atom.From);
                    var to = MathEx.Min(current.To, atom.To);
                    var period = new Period<T3>(from, to, value);
                    result.Add(period);
                }
            }

            return result;
        }

        // prerequisite: enumerator is from a valid periodization
        public static IEnumerable<Period<T3>> Combine<T1, T2, T3>(IEnumerable<Period<T1>> enumerable, Period<T2> atom, Func<T1, T2, T3> builder)
        {
            var result = new List<Period<T3>>();
            var enumerator = enumerable.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                // skip current
                // atom:              [------[
                // current:     [-----[
                if (current.To <= atom.From)
                {
                    continue;
                }
                // ==> atom.From < current.To

                // no more intersection
                // atom:       [------[
                // current:           [---------------[
                if (atom.To <= current.From)
                {
                    break;
                }
                // ==> current.From < atom.To

                // intersection
                // atom:        [------------[
                // current:         [---------------[
                // current:        [---[
                // current:   [---[
                // current:  [-----------------[
                if (current.From < atom.To)
                {
                    var value = builder(current.Value, atom.Value);
                    var from = current.From < atom.From
                        ? atom.From
                        : current.From;
                    var to = atom.To < current.To
                        ? atom.To
                        : current.To;
                    var period = new Period<T3>(from, to, value);
                    result.Add(period);
                }
            }

            return result;
        }

        // prerequisite: enumerator is from a valid periodization
        public static IEnumerable<Period<T>> Union<T>(IEnumerable<Period<T>> enumerable, Period<T> atom)
        {
            var result = new List<Period<T>>();

            var eqcmp = EqualityComparer<T>.Default;
            var enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                // current:              [-----[
                // atom:      [----------[ 
                if (atom.To <= current.From)
                {
                    NonInterlocked.Exchange(ref current, ref atom);
                }
                // current:   [----------[ 
                // atom:                 [-----[

                var same = eqcmp.Equals(current.Value, atom.Value);

                // current:      [A-------[ 
                // atom:               [B-----[
                if (!same && atom.From < current.To)
                {
                    throw new ArgumentException("Periods overlap");
                }

                // current:      [A-------[ 
                // atom:               [A-----[
                if (atom.From <= current.To && same)
                {
                    atom = new Period<T>(current.From, atom.To, atom.Value);
                }
                else
                {
                    // current:      [A-----[ 
                    // atom:                [B-----[
                    result.Add(current);
                }
            }

            result.Add(atom);

            return result;
        }
    }
}
