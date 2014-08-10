using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using HtmlAgilityPack;

using tursibNow.Model;
using tursibNow.Data;
using tursibNow.Core;

namespace tursibNow.Tests.TimingTest
{
    [TestFixture]
    public class TimingTest
    {
        // Object under test.
        Timing _timing;
        TimeTable _timeTable;

        public TimingTest()
        {
            _timing = new Timing(); 

            // Taken from 11 - Cedonia
            _timeTable = new TimeTable()
            {
                WeekDays = new List<DateTime>()
                {
                    new DateTime(2014, 08, 08, 5, 33, 00), // 5:33
                    new DateTime(2014, 08, 08, 5, 51, 00),
                    new DateTime(2014, 08, 08, 6, 15, 00),
                    new DateTime(2014, 08, 08, 6, 33, 00),
                    new DateTime(2014, 08, 08, 6, 53, 00),
                    new DateTime(2014, 08, 08, 7, 13, 00),
                    new DateTime(2014, 08, 08, 7, 33, 00),
                    new DateTime(2014, 08, 08, 7, 55, 00),
                    new DateTime(2014, 08, 08, 8, 13, 00),
                    new DateTime(2014, 08, 08, 8, 38, 00),
                    new DateTime(2014, 08, 08, 9, 8, 00),
                    new DateTime(2014, 08, 08, 9, 39, 00),
                    new DateTime(2014, 08, 08, 10, 8, 00),
                },
                Saturday = new List<DateTime>()
                {
                    new DateTime(2014, 08, 08, 6, 10, 00), // 6:10
                    new DateTime(2014, 08, 08, 6, 30, 00),
                    new DateTime(2014, 08, 08, 7, 20, 00),
                    new DateTime(2014, 08, 08, 7, 50, 00),
                    new DateTime(2014, 08, 08, 8, 30, 00),
                    new DateTime(2014, 08, 08, 9, 10, 00),
                    new DateTime(2014, 08, 08, 9, 50, 00),
                    new DateTime(2014, 08, 08, 10, 30, 00),
                    new DateTime(2014, 08, 08, 11, 10, 00),
                    new DateTime(2014, 08, 08, 11, 50, 00),
                    new DateTime(2014, 08, 08, 12, 30, 00),
                    new DateTime(2014, 08, 08, 13, 10, 00),
                    new DateTime(2014, 08, 08, 14, 0, 00),
                },
                Sunday = new List<DateTime>()
                {
                    new DateTime(2014, 08, 08, 6, 10, 00), // 6:10
                    new DateTime(2014, 08, 08, 6, 30, 00),
                    new DateTime(2014, 08, 08, 7, 20, 00),
                    new DateTime(2014, 08, 08, 7, 50, 00),
                    new DateTime(2014, 08, 08, 8, 30, 00),
                    new DateTime(2014, 08, 08, 9, 10, 00),
                    new DateTime(2014, 08, 08, 9, 50, 00),
                    new DateTime(2014, 08, 08, 10, 30, 00),
                    new DateTime(2014, 08, 08, 11, 10, 00),
                    new DateTime(2014, 08, 08, 11, 50, 00),
                    new DateTime(2014, 08, 08, 12, 30, 00),
                    new DateTime(2014, 08, 08, 13, 10, 00),
                    new DateTime(2014, 08, 08, 14, 0, 00),
                }
            };
        }

        [Test]
        public void Timing_ReturnNumberOfTimes()
        {
            // Weekdays
            DateTime timeMockMonday = new DateTime(2014, 08, 11, 6, 15, 00); // 6:15 - Monday
            var dateTimeCalculatorMonday = new DateTimeCalculatorMock(timeMockMonday);
            // Change the original time calculator
            _timing.DateTimeCalculator = dateTimeCalculatorMonday;
            // Should get the weekdays, 6:30, 7:20 and 7:50
            var result1 = _timing.NextTimes(_timeTable, 3);
            var result2 = _timing.NextTimes(_timeTable, 15);

            // Times to be extracted
            Assert.AreEqual(3, result1.Count());
            // Times asked is greated than the times left
            Assert.AreEqual(11, result2.Count());


            // Saturday
            DateTime timeMockSaturday = new DateTime(2014, 08, 16, 12, 0, 00); // 12:00 - Saturday
            var dateTimeCalculatorSaturday = new DateTimeCalculatorMock(timeMockSaturday);
            _timing.DateTimeCalculator = dateTimeCalculatorSaturday;

            var result3 = _timing.NextTimes(_timeTable, 3);
            var result4 = _timing.NextTimes(_timeTable, 5);

            Assert.AreEqual(3, result3.Count());
            Assert.AreEqual(3, result4.Count());


            // Sunday
            DateTime timeMockSunday = new DateTime(2014, 08, 17, 15, 0, 00); // 15:00 - Saturday
            var dateTimeCalculatorSunday = new DateTimeCalculatorMock(timeMockSunday);
            _timing.DateTimeCalculator = dateTimeCalculatorSunday;

            var result5 = _timing.NextTimes(_timeTable, 3);

            // No buses after this hour
            Assert.AreEqual(0, result5.Count());
        }
    }
}