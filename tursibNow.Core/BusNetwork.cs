using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using tursibNow.Model;
using tursibNow.Data;

namespace tursibNow.Core
{
    /// <summary>
    /// Retrieves a bus network from the StoragePath. If the path is empty, a new one is created. 
    /// Uses the path specified by StoragePath to save/update the bus info.
    /// Use:
    /// - Set the StoragePath to a folder location
    /// - Retrieve the bus network with the Buses attribute
    /// </summary>
    public class BusNetwork
    {
        // Storage path for files containing the bus network info
        public static string StoragePath { get; set; }

        private static List<Bus> _buses;
        public static List<Bus> Buses 
        { 
            get 
            {
                if (_buses == null)
                {
                    Update();
                }
                return _buses; 
            } 
        }

        /// <summary>
        /// Returns a list of all station names
        /// </summary>
        public static string[] StationNames
        {
            get
            {
                return SearchUtils.StationNames(SearchUtils.StationList(Buses));
            }
        }

        /// <summary>
        /// Returns true if the bus network contains a station with the given name
        /// </summary>
        public static bool ContainsStationName(string stationName)
        {
            return Buses.ContainsStationName(stationName);
        }

        /// <summary>
        /// Returns a list of buses that have the given route
        /// </summary>
        public static IEnumerable<Tuple<Bus, Station>> BusRoutes(string departure, string arrival)
        {
            return Buses.BusRoutes(departure, arrival);
        }

        /// <summary>
        /// Returns the station timetable that has the given route
        /// </summary>
        public static TimeTable RouteTimetable(Bus bus, string departure, string arrival)
        {
            return bus.SearchUtilsRouteTimetable(departure, arrival);
        }
        
        static void Update()
        {
            var htmlService = new HtmlServiceTursibRo();
            var busSaveRetrieve = new BusNetworkSaveRetrieve(StoragePath, htmlService);
            _buses = busSaveRetrieve.Buses as List<Bus>;
        }
    }
}