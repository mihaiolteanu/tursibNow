using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tursibNow.Model
{
    //bus time table 
    //every bus has a timetable for each station
    public class TimeTable
    {
        //different time tables for monday-friday, saturday and sunday, respectively
        public IEnumerable<DateTime> WeekDays { get; set; }
        public IEnumerable<DateTime> Saturday { get; set; }
        public IEnumerable<DateTime> Sunday   { get; set; }
    }
}
