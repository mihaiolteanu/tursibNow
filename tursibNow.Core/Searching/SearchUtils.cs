using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using tursibNow.Model;

namespace tursibNow.Core
{
    public static class SearchUtils
    {
        /// <summary>
        /// Returns a list of all the available bus stations from a list of buses
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Station> Stations(IEnumerable<Bus> buses)
        {
            List<Station> result = new List<Station>();
            foreach (var bus in buses)
            {
                foreach (var dStation in bus.DirectStations)
                {
                    result.Add(dStation);
                }

                foreach (Station rStation in bus.ReverseStations)
                {
                    result.Add(rStation);
                }
            }
            return result;
        }

        /// <summary>
        /// Searches a list of buses for all the buses that have a given route.
        /// </summary>
        /// <param name="buses"></param>
        /// <param name="departure"></param>
        /// <param name="arrival"></param>
        /// <returns></returns>
        public static IEnumerable<Bus> Buses(IEnumerable<Bus> buses, Station departure, Station arrival)
        {
            return from bus in buses
                   where (bus.DirectStations.Where(s => s.Name == departure.Name).Any() &&
                         bus.DirectStations.Where(s => s.Name == arrival.Name).Any()) ||
                         (bus.ReverseStations.Where(s => s.Name == departure.Name).Any() &&
                         bus.ReverseStations.Where(s => s.Name == arrival.Name).Any())
                   select bus;
        }
    }
}