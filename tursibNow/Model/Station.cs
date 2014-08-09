using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tursibNow.Model
{
    //one bus stop
    //every bus bas multiple stops
    public class Station : IHasLabel, IComparable<Station>
    {
        //name of the station
        public string Name { get; set; }
        public TimeTable TimeTable { get; set; }

        public string Label
        {
            get { return Name[0].ToString(); }
        }

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
