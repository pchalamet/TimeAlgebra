using System;
using System.Linq;
using NFluent;
using NUnit.Framework;

namespace TimeAlgebra.Tests
{
    [TestFixture]
    public class PeriodizationUnionTests
    {
        [Test]
        public void Check_empty_has_no_element()
        {
            var tempo = Periodization<int>.Empty;
            Check.That(tempo.Periods).HasSize(0);
        }

        [Test]
        public void Check_append_element_to_empty_list_equals_element()
        {
            var date1 = new DateTime(2014, 12, 29);
            var period = Period<int>.Day(date1, 1);
            var res = Periodization<int>.Empty.Append(period);
            Check.That(res.Periods).HasSize(1);

            var singlePeriod = res.Periods.Single();
            Check.That(singlePeriod.From).Equals(date1);
            Check.That(singlePeriod.To).Equals(date1.AddDays(1));
            Check.That(singlePeriod.Value).Equals(1);
        }

        [Test]
        public void Check_append_element_at_beginning_of_list()
        {
            var date1 = new DateTime(2015, 1, 2);
            var period1 = Period<int>.Day(date1, 1);
            var res1 = Periodization<int>.Empty.Append(period1);

            var date2 = new DateTime(2015, 12, 29);
            var period2 = Period<int>.Day(date2, 2);
            var res2 = res1.Append(period2);

            Check.That(res2.Periods).HasSize(2);

            var testPeriod1 = res2.Periods.First();
            Check.That(testPeriod1.From).Equals(date1);
            Check.That(testPeriod1.To).Equals(date1.AddDays(1));
            Check.That(testPeriod1.Value).Equals(1);

            var testPeriod2 = res2.Periods.Last();
            Check.That(testPeriod2.From).Equals(date2);
            Check.That(testPeriod2.To).Equals(date2.AddDays(1));
            Check.That(testPeriod2.Value).Equals(2);
        }

        [Test]
        public void Check_append_element_at_end_of_list()
        {
            var date1 = new DateTime(2014, 12, 29);
            var period1 = Period<int>.Day(date1, 1);
            var res1 = Periodization<int>.Empty.Append(period1);

            var date2 = new DateTime(2015, 1, 1);
            var period2 = Period<int>.Day(date2, 2);
            var res2 = res1.Append(period2);

            Check.That(res2.Periods).HasSize(2);

            var testPeriod1 = res2.Periods.First();
            Check.That(testPeriod1.From).Equals(date1);
            Check.That(testPeriod1.To).Equals(date1.AddDays(1));
            Check.That(testPeriod1.Value).Equals(1);

            var testPeriod2 = res2.Periods.Last();
            Check.That(testPeriod2.From).Equals(date2);
            Check.That(testPeriod2.To).Equals(date2.AddDays(1));
            Check.That(testPeriod2.Value).Equals(2);
        }

        [Test]
        public void Check_append_element_at_middle_of_list()
        {
            var date1 = new DateTime(2014, 12, 29);
            var period1 = Period<int>.Day(date1, 1);
            var res1 = Periodization<int>.Empty.Append(period1);

            var date2 = new DateTime(2015, 1, 3);
            var period2 = Period<int>.Day(date2, 2);
            var res2 = res1.Append(period2);

            var date3 = new DateTime(2015, 1, 1);
            var period3 = Period<int>.Day(date3, 3);
            var res3 = res2.Append(period3);

            Check.That(res3.Periods).HasSize(3);

            var testPeriod1 = res3.Periods.First();
            Check.That(testPeriod1.From).Equals(date1);
            Check.That(testPeriod1.To).Equals(date1.AddDays(1));
            Check.That(testPeriod1.Value).Equals(1);

            var testPeriod2 = res3.Periods.ElementAt(1);
            Check.That(testPeriod2.From).Equals(date3);
            Check.That(testPeriod2.To).Equals(date3.AddDays(1));
            Check.That(testPeriod2.Value).Equals(3);

            var testPeriod3 = res3.Periods.Last();
            Check.That(testPeriod3.From).Equals(date2);
            Check.That(testPeriod3.To).Equals(date2.AddDays(1));
            Check.That(testPeriod3.Value).Equals(2);
        }

        [Test]
        public void Check_period_overlap()
        {
            var date1 = new DateTime(2014, 12, 29);
            var period1 = Period<int>.Day(date1, 1);
            var res1 = Periodization<int>.Empty.Append(period1);

            var date2 = new DateTime(2014, 12, 29);
            var period2 = Period<int>.Day(date2, 2);

            Check.ThatCode(() => res1.Append(period2)).Throws<ArgumentException>().WithMessage("Periods overlap");
        }

        [Test]
        public void Check_period_overlap_but_merge()
        {
            var date1 = new DateTime(2014, 12, 29);
            var period1 = Period<int>.Day(date1, 1);
            var res1 = Periodization<int>.Empty.Append(period1);

            var date2 = new DateTime(2014, 12, 29);
            var period2 = Period<int>.Days(date2, 2, 1);

            var res2 = res1.Append(period2);
            Check.That(res2.Periods).HasSize(1);
            Check.That(res2.Periods.Single().From).Equals(date1);
            Check.That(res2.Periods.Single().To).Equals(period2.To);
            Check.That(res2.Periods.Single().Value).Equals(1);
        }

        [Test]
        public void Check_period_merge()
        {
            var date1 = new DateTime(2015, 1, 1);
            var period1 = Period<int>.Day(date1, 1);
            var res1 = Periodization<int>.Empty.Append(period1);
            Console.WriteLine(res1);

            var date2 = new DateTime(2015, 1, 3);
            var period2 = Period<int>.Day(date2, 1);
            var res2 = res1.Append(period2);
            Console.WriteLine(res2);

            var date3 = new DateTime(2015, 1, 2);
            var period3 = Period<int>.Day(date3, 1);
            var res3 = res2.Append(period3);
            Console.WriteLine(res3);

            var date4 = new DateTime(2015, 1, 4);
            var period4 = Period<int>.Day(date4, 2);
            var res4 = res3.Append(period4);
            Console.WriteLine(res4);

            Check.That(res4.Periods).HasSize(2);

            var testPeriod1 = res4.Periods.First();
            Check.That(testPeriod1.From).Equals(period1.From);
            Check.That(testPeriod1.To).Equals(period2.To);
            Check.That(testPeriod1.Value).Equals(1);

            var testPeriod2 = res4.Periods.Last();
            Check.That(testPeriod2.From).Equals(period4.From);
            Check.That(testPeriod2.To).Equals(period4.To);
            Check.That(testPeriod2.Value).Equals(2);
        }
    }
}
