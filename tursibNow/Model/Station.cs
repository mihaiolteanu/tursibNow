using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tursibNow.Model
{
    //one bus stop
    //every bus bas multiple stops
    public class Station
    {
        //name of the station
        public string Name { get; set; }
        public TimeTable TimeTable { get; set; }
    }
}
