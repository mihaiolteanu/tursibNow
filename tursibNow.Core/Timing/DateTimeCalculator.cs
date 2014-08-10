using System;

namespace tursibNow.Core
{
    public class DateTimeCalculator : IDateTimeCalculator
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}