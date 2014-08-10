using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tursibNow.Model
{
    /// <summary>
    /// Represents a timetable for a given station.
    /// Buses have different timetable depending on the day of the week.
    /// </summary>
    [Serializable]
    public class TimeTable
    {
        /// <summary>
        /// Monday-Friday timetable.
        /// </summary>
        public IEnumerable<DateTime> WeekDays { get; set; }
        
        /// <summary>
        /// Saturday timetable.
        /// </summary>
        public IEnumerable<DateTime> Saturday { get; set; }
        
        /// <summary>
        /// Sunday timetable.
        /// </summary>
        public IEnumerable<DateTime> Sunday   { get; set; }
    }
}
