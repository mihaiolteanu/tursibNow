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
    static class SearchUtils
    {
        /// <summary>
        /// Returns a list of all unique station names
        /// </summary>
        public static string[] StationNames(IEnumerable<Station> stations)
        {
            // Get unique station names
            var stationNames = (from station in stations
                                select station.Name)
                                .Distinct();
            return stationNames.ToArray();
        }

        /// <summary>
        /// Searches a list of buses for all the buses that have a given route.
        /// </summary>
        /// <returns>bus and bus station object, where station represents the departure station</returns>
        public static IEnumerable<Tuple<Bus, Station>> BusRoutes(this IEnumerable<Bus> buses, string departure, string arrival)
        {
            List<Tuple<Bus, Station>> result = new List<Tuple<Bus, Station>>();

            foreach (var bus in buses)
            {
                var departurePresent = bus.DirectStations.Where(st => st.Name == departure).Any();
                var arrivalPresent = bus.DirectStations.Where(st => st.Name == arrival).Any();

                // The route exists 
                if (departurePresent && arrivalPresent)
                {
                    // Is the arrival station after the departure station
                    var departureIndex = bus.DirectStations.ToList().FindIndex(st => st.Name == departure);
                    var arrivalIndex = bus.DirectStations.ToList().FindIndex(st => st.Name == arrival);
                    if (arrivalIndex > departureIndex)
                    {
                        Tuple<Bus, Station> found = new Tuple<Bus, Station>(bus, bus.DirectStations.Where(st => st.Name == departure).First());
                        result.Add(found);
                    }
                }


                var departurePresentR = bus.ReverseStations.Where(st => st.Name == departure).Any();
                var arrivalPresentR = bus.ReverseStations.Where(st => st.Name == arrival).Any();

                // The route exists 
                if (departurePresentR && arrivalPresentR)
                {
                    // Is the arrival station after the departure station
                    var departureIndex = bus.ReverseStations.ToList().FindIndex(st => st.Name == departure);
                    var arrivalIndex = bus.ReverseStations.ToList().FindIndex(st => st.Name == arrival);
                    if (arrivalIndex > departureIndex)
                    {
                        Tuple<Bus, Station> found = new Tuple<Bus, Station>(bus, bus.ReverseStations.Where(st => st.Name == departure).First());
                        result.Add(found);
                    }
                }
            }
            return result;

            //return from bus in buses
            //       where (bus.DirectStations.Where(s => s.Name.ToLower() == departure.ToLower()).Any() &&
            //             bus.DirectStations.Where(s => s.Name.ToLower() == arrival.ToLower()).Any()) ||
            //             (bus.ReverseStations.Where(s => s.Name.ToLower() == departure.ToLower()).Any() &&
            //             bus.ReverseStations.Where(s => s.Name.ToLower() == arrival.ToLower()).Any())
            //       select bus;
        }

        /// <summary>
        /// Returns a list of all the available bus stations from a list of buses
        /// </summary>
        public static IEnumerable<Station> StationList(IEnumerable<Bus> buses)
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
        /// Returns true if the list of buses contains at least one station with the stationName
        /// </summary>
        public static bool ContainsStationName(this IEnumerable<Bus> buses, string stationName)
        {
            var stationList = StationList(buses);
            return stationList.ContainsStationName(stationName);
        }

        /// <summary>
        /// Returns true if the list of stations contains at least one station with the stationName
        /// </summary>
        public static bool ContainsStationName(this IEnumerable<Station> stations, string stationName)
        {
            return (from station in stations
                    where (station.Name == stationName)
                    select station.Name)
                    .Any();
        }

        /// <summary>
        /// Returns the station timetable that has the given route
        /// </summary>
        public static TimeTable SearchUtilsRouteTimetable(this Bus bus, string departure, string arrival)
        {
            var directDeparture = bus.DirectStations.Where(st => st.Name == departure);
            var directArrival = bus.DirectStations.Where(st => st.Name == arrival);

            if (directDeparture != null && directArrival != null)
            {
                // Route found in directe routes
                return directDeparture.First().TimeTable;
            }

            var reverseDeparture = bus.ReverseStations.Where(st => st.Name == departure);
            var reverseArrival = bus.ReverseStations.Where(st => st.Name == arrival);

            if (reverseDeparture != null && reverseArrival != null)
            {
                // Route found in reverse routes
                return reverseDeparture.First().TimeTable;
            }
            // Route not found
            return new TimeTable();
        }
    }
}