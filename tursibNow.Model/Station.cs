using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tursibNow.Model
{
    /// <summary>
    /// Represents one bus stations, complete with the station name and timetable.
    /// </summary>
    public class Station : IHasLabel, IComparable<Station>
    {
        /// <summary>
        /// Station name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Station timetable.
        /// </summary>
        public TimeTable TimeTable { get; set; }

        /// <summary>
        /// IHasLabel implemewntation
        /// </summary>
        public string Label
        {
            get { return Name[0].ToString(); }
        }

        /// <summary>
        /// IComparable implementation
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Station other)
        {
            return Name.CompareTo(other.Name);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// same station can be an arrival or departure point for a bus
    /// </summary>
    public enum Direction
    {
        dus, //direct (romanian)
        intors //reverse (romanian)
    }
}
