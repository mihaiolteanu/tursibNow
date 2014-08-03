using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

using tursibNow.HtmlService;

namespace tursibNow.Model
{
    /// <summary>
    /// retrieves the bus network information from html pages
    /// </summary>
    public class BusNetworkHtml : IBusNetwork, IEnumerable<Bus>
    {
        //used to retrieve the html from which to extract the bus info
        IHtmlService _htmlService;
        List<Bus> _buses = new List<Bus>();
        public IEnumerable<Bus> Buses 
        {
            get { return _buses; }
        }

        /// <summary>
        /// builds a list of buses from a html file
        /// </summary>
        /// <param name="url"></param>
        public BusNetworkHtml(IHtmlService htmlService)
        {
            _htmlService = htmlService;
            //get the names and numbers of buses
            HtmlDocument busOverview = _htmlService.BusOverview();

            //get the list of buses in the form
            //<a href="/traseu/1"><strong>Cimitir  - Obi/Viile Sibiului</strong></a>
            //see the BusOverview.htm for an example
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

            //add a new bus for each element in the query
            //for each bus number, we have to get a new html page from the htmlservice
            foreach (var busNumberName in busNumberNameList)
            {
                Bus bus = new Bus();

                //get the number and name of the bus
                bus.Number = busNumberName.busNo;
                bus.Name = busNumberName.busName;

                //get the bus stations based on the bus number / direct
                List<Station> directStations = GetStationsComplete(bus.Number, Direction.dus);
                bus.DirectStations = directStations;

                //get the bus stations based on the bus number / reverse
                List<Station> reverseStations = GetStationsComplete(bus.Number, Direction.intors);
                bus.ReverseStations = reverseStations;

                //we have all the information necessary to add a new bus
                _buses.Add(bus);
            }
        }
                
        

        /// <summary>
        /// get all station names for a particular bus and direction
        /// </summary>
        /// <param name="busNo">bus number for which to return the station names</param>
        /// <param name="direction">direct/reverse</param>
        /// <returns>list of all the station names</returns>
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
        /// gets all the bus stations (containing names and the complete timetable) in a given direction
        /// </summary>
        /// <param name="busNumber"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private List<Station> GetStationsComplete(string busNumber, Direction direction)
        {
            List<Station> stationsToReturn = new List<Station>();

            //get a list of all the station names in a given direction
            List<string> stationsNameList = GetStationNames(busNumber, direction).ToList();

            foreach (string station in stationsNameList)
            {
                //get the complete timetable for the station
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
        /// gets the complete timetable for a bus station for all the days of the week
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
        /// gets the timetable as a list of DateTime for a bus station, in a particular direction and for a particular day
        /// </summary>
        /// <param name="busNumber">bus number for which we want the timetable</param>
        /// <param name="stationNumber">station number for which we want the timetable</param>
        /// <param name="direction">direct/reverse</param>
        /// <param name="timeTableDay">the day of the week for which the timetable is wanted</param>
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
        
        #region IEnumerable<Bus> implementation
        public IEnumerator<Bus> GetEnumerator()
        {
            foreach (Bus bus in Buses)
            {
                yield return bus;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}