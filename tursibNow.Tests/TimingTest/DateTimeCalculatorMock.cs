using System;
using tursibNow.Core;

namespace tursibNow.Tests.TimingTest
{
    /// <summary>
    /// Return the dateTime of choice
    /// </summary>
    class DateTimeCalculatorMock : IDateTimeCalculator
    {
        DateTime _dateTime;
        public DateTime Now()
        {
            return _dateTime;
        }

        public DateTimeCalculatorMock(DateTime dateTime)
        {
            _dateTime = dateTime;
        }
    }
}