using System;
using System.Collections.Generic;
using System.Linq;
using tursibNow.Model;

namespace tursibNow.Core
{
    /// <summary>
    /// Calculates time differences related to bus timetables
    /// </summary>
    public class Timing
    {
        private IDateTimeCalculator _dateTimeCalculator;
        
        // Modify the date time calculator used to retrieve the current time
        public IDateTimeCalculator DateTimeCalculator { set { _dateTimeCalculator = value; } }

        public Timing()
        {
            var a = 
            // The default date time calculator
            _dateTimeCalculator = new DateTimeCalculator();
        }
        
        /// <summary>
        /// Returns a list of 'count' DateTime objects, greater than the current time 
        /// from a TimeTable object
        /// </summary>
        /// <param name="timeTable"></param>
        /// <param name="count">How many items to return, greater than the current time</param>
        /// <returns></returns>
        public IEnumerable<DateTime> NextTimes(TimeTable timeTable, int count)
        {
            DateTime timeNow = _dateTimeCalculator.Now();

            switch (timeNow.DayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    return NextTimesFromList(timeTable.WeekDays, timeNow, count);

                case DayOfWeek.Saturday:
                    return NextTimesFromList(timeTable.Saturday, timeNow, count);
                
                case DayOfWeek.Sunday:
                    return NextTimesFromList(timeTable.Sunday, timeNow, count);
                
                default:
                    return new List<DateTime>();
            }
        }

        /// <summary>
        /// Returns the next count DateTime objects from a DateTime list, such that
        /// they are greater than the timeNow
        /// </summary>
        /// <param name="dateTimes">List of DateTime objects</param>
        /// <param name="timeNow"></param>
        /// <param name="count">How many times to return</param>
        /// <returns></returns>
        private IEnumerable<DateTime> NextTimesFromList(IEnumerable<DateTime> dateTimes, DateTime timeNow, int count)
        {
            // Get times greater than the current time
            var nextDateTimes = from dateTime in dateTimes
                                where ((dateTime.Hour > timeNow.Hour) ||
                                       (dateTime.Hour == timeNow.Hour) && (dateTime.Minute >= timeNow.Minute))
                                select dateTime;


            // Get only count number of times greater than the current time
            int nextDateTimesCount = nextDateTimes.ToList().Count;
            // Can't get more times than available
            if (count > nextDateTimesCount)
            {
                // Get all of them instead
                count = nextDateTimesCount;
            }

            // Get count number of items from the list of nextDateTimes
            var result = new List<DateTime>();
            foreach(var element in nextDateTimes)
            {
                if (count > 0)
                {
                    result.Add(element);
                    count--;
                }
            }
            return result;
        }
    }
}