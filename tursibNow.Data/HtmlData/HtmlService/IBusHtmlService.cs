using HtmlAgilityPack;
using tursibNow.Model;

namespace tursibNow.Data
{
    /// <summary>
    /// Retrieves html pages containing information about the buses.
    /// </summary>
    public interface IBusHtmlService
    {
        /// <summary>
        /// Get a html page with all the bus numbers and names (see the tursibHtmlSamples/BusOverview.htm for an example, or tursib.ro/trasee).
        /// </summary>
        HtmlDocument BusOverview();

        /// <summary>
        /// Get a html page for a certain bus that contains all the bus stations (see tursib.ro/traseu/11, for example)
        /// </summary>
        /// <param name="busNumber">the bus number for which the html page containing all the stations should be returned</param>
        /// <returns></returns>
        HtmlDocument BusStations(string busNumber);

        /// <summary>
        /// Get a html page for a certain bus and a certain station number (station numbers are counted from 0 to the last station)
        /// that contains the bus timetable for that station. 
        /// </summary>
        /// <param name="busNumber"></param>
        /// <param name="stationNumber"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        HtmlDocument BusTimetable(string busNumber, string stationNumber, Direction direction);
    }
}