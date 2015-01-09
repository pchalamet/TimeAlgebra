using System;
using System.Linq;
using NFluent;
using NUnit.Framework;

namespace TimeAlgebra.Tests
{
    [TestFixture]
    public class PeriodizationIntersectionTests
    {
        [Test]
        public void Check_intersection_before()
        {
            //   3 4 5 6
            var date1 = new DateTime(2015, 1, 3);
            var period1 = Period<int>.Days(date1, 3, 5);
            var res1 = Periodization<int>.Empty.Append(period1);

            // 2 3 4 5
            var date2 = new DateTime(2015, 1, 2);
            var period2 = Period<bool>.Days(date2, 3, true);
            var res3 = res1.Intersect(period2, Tuple.Create);

            Check.That(res3.Periods).HasSize(1);
            Check.That(res3.Periods.Single().From).Equals(new DateTime(2015, 1, 3));
            Check.That(res3.Periods.Single().To).Equals(new DateTime(2015, 1, 5));
            Check.That(res3.Periods.Single().Value).Equals(Tuple.Create(5, true));
        }

        [Test]
        public void Check_intersection_after()
        {
            //   3 4 5 6
            var date1 = new DateTime(2015, 1, 3);
            var period1 = Period<int>.Days(date1, 3, 5);

            // 2 3 4 5
            var date2 = new DateTime(2015, 1, 2);
            var period2 = Period<bool>.Days(date2, 3, true);
            var res2 = Periodization<bool>.Empty.Append(period2);
            var res3 = res2.Intersect(period1, Tuple.Create);

            Check.That(res3.Periods).HasSize(1);
            Check.That(res3.Periods.Single().From).Equals(new DateTime(2015, 1, 3));
            Check.That(res3.Periods.Single().To).Equals(new DateTime(2015, 1, 5));
            Check.That(res3.Periods.Single().Value).Equals(Tuple.Create(true, 5));
        }

        [Test]
        public void Check_intersection_overlap()
        {
            // 1 2 3 4 5 6 7 8 9
            var date1 = new DateTime(2015, 1, 1);
            var period1 = Period<int>.Days(date1, 8, 5);

            //   2 3 4     7 8
            var date2 = new DateTime(2015, 1, 2);
            var period2 = Period<bool>.Days(date2, 2, true);
            var res2 = Periodization<bool>.Empty.Append(period2);

            var date3 = new DateTime(2015, 1, 7);
            var period3 = Period<bool>.Day(date3, false);
            var res3 = res2.Append(period3);
            var res4 = res3.Intersect(period1, Tuple.Create);

            Check.That(res4.Periods).HasSize(2);

            Check.That(res4.Periods.First().From).Equals(new DateTime(2015, 1, 2));
            Check.That(res4.Periods.First().To).Equals(new DateTime(2015, 1, 4));
            Check.That(res4.Periods.First().Value).Equals(Tuple.Create(true, 5));

            Check.That(res4.Periods.Last().From).Equals(new DateTime(2015, 1, 7));
            Check.That(res4.Periods.Last().To).Equals(new DateTime(2015, 1, 8));
            Check.That(res4.Periods.Last().Value).Equals(Tuple.Create(false, 5));
        }

        [Test]
        public void Check_no_intersection()
        {
            var date1 = new DateTime(2015, 1, 1);
            var period1 = Period<int>.Day(date1, 1);
            var res1 = Periodization<int>.Empty.Append(period1);

            var date2 = new DateTime(2015, 2, 1);
            var period2 = Period<bool>.Day(date2, true);

            var res3 = res1.Intersect(period2, Tuple.Create);

            Check.That(res3.Periods).HasSize(0);
        }

        [Test]
        public void Check_same_intersection()
        {
            var date1 = new DateTime(2015, 1, 1);
            var period1 = Period<int>.Days(date1, 3, 5);
            var res1 = Periodization<int>.Empty.Append(period1);

            var date2 = new DateTime(2015, 1, 1);
            var period2 = Period<bool>.Days(date2, 3, true);

            var res3 = res1.Intersect(period2, Tuple.Create);

            Check.That(res3.Periods).HasSize(1);
            Check.That(res3.Periods.Single().Value).Equals(Tuple.Create(5, true));
        }
    }
}
