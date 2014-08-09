using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using tursibNow.Model;

namespace tursibNow.Data
{
    /// <summary>
    /// Retrieves the bus network information from html pages
    /// </summary>
    public class BusNetworkHtml : IBusNetwork
    {
        // Used to retrieve the html page from which to extract the bus info
        IHtmlService _htmlService;
        
        List<Bus> _buses = new List<Bus>();
        public IEnumerable<Bus> Buses { get { return _buses; } }

        /// <summary>
        /// Builds a list of buses from a html file
        /// </summary>
        /// <param name="htmlService">Html service used to retrieve html pages</param>
        public BusNetworkHtml(IHtmlService htmlService)
        {
            _htmlService = htmlService;
            // Get the names and numbers of buses as a html page
            HtmlDocument busOverview = _htmlService.BusOverview();

            // Gget the list of buses in the form
            // <a href="/traseu/1"><strong>Cimitir  - Obi/Viile Sibiului</strong></a>
            // See the BusOverview.htm for an example.
            var busNumberNameList =
                from busLink in busOverview.DocumentNode.Descendants("a")
                where ((busLink.Attributes["href"] != null) && (busLink.Attributes["href"].Value.Contains("/traseu/"))) &&
                      ((busLink.ParentNode.Attributes["class"] != null) && (busLink.ParentNode.Attributes["class"].Value == "denumire"))
                //bus number contained in the href tag: href = "/traseu/busNumber"
                let busNo = HtmlServiceUtil.ExtractBusNumber(busLink.Attributes["href"].Value)          
                let busName = busLink.InnerHtml.Replace("<strong>", "").Replace("</strong>", "")
                select new
                {
                    busNo,
                    busName
                };

            // Add a new bus for each element in the query.
            // For each bus number, a new html page from the htmlservice must be fetched
            foreach (var busNumberName in busNumberNameList)
            {
                Bus bus = new Bus();

                // Get the number and name of the bus.
                bus.Number = busNumberName.busNo;
                bus.Name = busNumberName.busName;

                // Get the bus stations based on the bus number / direct.
                List<Station> directStations = GetStationsComplete(bus.Number, Direction.dus);
                bus.DirectStations = directStations;

                // Get the bus stations based on the bus number / reverse.
                List<Station> reverseStations = GetStationsComplete(bus.Number, Direction.intors);
                bus.ReverseStations = reverseStations;

                // All the information necessary to add a new bus is now available
                _buses.Add(bus);
            }
        }

        /// <summary>
        /// Ret all station names for a particular bus and direction.
        /// </summary>
        /// <param name="busNo">Bus number for which to return the station names</param>
        /// <param name="direction">Direct/reverse</param>
        /// <returns>List of all the station names</returns>
        private IEnumerable<string> GetStationNames(string busNo, Direction direction)
        {
            HtmlDocument doc = _htmlService.BusStations(busNo);

            var stations =
                from statieLink in doc.DocumentNode.Descendants("a")
                where ((statieLink.Attributes["class"] != null) && (statieLink.Attributes["class"].Value == "statie-link")) &&
                      ((statieLink.Attributes["href"] != null) && (statieLink.Attributes["href"].Value.Contains(direction.ToString())))
                select statieLink.InnerHtml.ToLower();

            return stations;
        }

        /// <summary>
        /// Gets all the bus stations (containing names and the complete timetable) in a given direction.
        /// </summary>
        /// <param name="busNumber"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private List<Station> GetStationsComplete(string busNumber, Direction direction)
        {
            List<Station> stationsToReturn = new List<Station>();

            // Get a list of all the station names in a given direction.
            List<string> stationsNameList = GetStationNames(busNumber, direction).ToList();

            foreach (string station in stationsNameList)
            {
                // Get the complete timetable for the station.
                TimeTable timeTable = GetTimeTableComplete(busNumber, stationsNameList.IndexOf(station).ToString(), direction);

                //we have all the info for a new station
                Station newStation = new Station()
                {
                    Name = station,
                    TimeTable = timeTable
                };

                stationsToReturn.Add(newStation);
            }
            return stationsToReturn;
        }

        /// <summary>
        /// Gets the complete timetable for a bus station for all the days of the week.
        /// </summary>
        /// <param name="busNumber"></param>
        /// <param name="stationNumber"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private TimeTable GetTimeTableComplete(string busNumber, string stationNumber, Direction direction)
        {
            HtmlDocument doc = _htmlService.BusTimetable(busNumber, stationNumber, direction);

            List<DateTime> timetableWeekdays = GetTimeTableDay(busNumber, stationNumber, direction, TimeTableDay.Weekdays, doc);
            List<DateTime> timetableSaturday = GetTimeTableDay(busNumber, stationNumber, direction, TimeTableDay.Saturday, doc);
            List<DateTime> timetableSunday = GetTimeTableDay(busNumber, stationNumber, direction, TimeTableDay.Sunday, doc);

            TimeTable completeTimeTable = new TimeTable
            {
                WeekDays = timetableWeekdays,
                Saturday = timetableSaturday,
                Sunday = timetableSunday
            };

            return completeTimeTable;
        }

        /// <summary>
        /// Gets the timetable as a list of DateTime for a bus station, in a particular direction and for a particular day.
        /// </summary>
        /// <param name="busNumber">Bus number for which we want the timetable</param>
        /// <param name="stationNumber">Station number for which we want the timetable</param>
        /// <param name="direction">Direct/reverse</param>
        /// <param name="timeTableDay">The day of the week for which the timetable is wanted</param>
        /// <returns></returns>
        private List<DateTime> GetTimeTableDay(string busNumber, string stationNumber, Direction direction, string timeTableDay, HtmlDocument doc)
        {            
            var timeTable =
                from links in doc.DocumentNode.Descendants("div")
                where ((links.ParentNode.Attributes["class"] != null) && (links.ParentNode.Attributes["class"].Value.Contains("plecari"))) &&
                      ((links.Attributes["class"] != null) && (links.Attributes["class"].Value == "h p0")) &&
                      //sometimes the previous sibling is just text (from a new line, for example) => get previous.previous sibling
                      ((links.ParentNode.PreviousSibling.InnerHtml == timeTableDay) || (links.ParentNode.PreviousSibling.PreviousSibling.InnerHtml == timeTableDay))
                select links.InnerHtml;

            return HtmlServiceUtil.StringsToDateTimes(timeTable.ToList());
        }
    }
}