using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tursibNow.Model
{
    public class Bus
    {
        //example "11: Cedonia - Sc Continental"
        public string Number { get; set; }
        public string Name { get; set; }

        //every bus has multiple stops
        public IEnumerable<Station> Stations { get; set; }
    }
}
