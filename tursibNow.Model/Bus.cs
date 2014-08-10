using System;
using System.Collections.Generic;

namespace tursibNow.Model
{
    /// <summary>
    /// Represents a single bus, complete with bus name, number and stations
    /// </summary>
    [Serializable]
    public class Bus
    {
        //example "11: Cedonia - Sc Continental"
        /// <summary>
        /// Bus number.
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Bus name.
        /// </summary>
        public string Name { get; set; }

        //every bus has multiple stops for two different directions (often times, the names differ between the direct and reverse direction)
        public IEnumerable<Station> DirectStations { get; set; }
        public IEnumerable<Station> ReverseStations { get; set; }
    }
}