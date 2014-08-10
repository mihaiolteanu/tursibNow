using System;
using HtmlAgilityPack;
using tursibNow.Model;

namespace tursibNow.Data
{
    /// <summary>
    /// Implements the retrieval of html web pages from www.tursib.ro.
    /// </summary>
    public class HtmlServiceTursibRo : IBusHtmlService
    {
        // Tursib official web page - starting place for all html pages to be retrieved
        Uri tursibUri = new Uri("http://tursib.ro/");

        /// <summary>
        /// Returns a html page with all the buses available from tursib.
        /// </summary>
        public HtmlDocument BusOverview()
        {
            UriBuilder uriBuilder = new UriBuilder(tursibUri);
            uriBuilder.Path += "/trasee";
            return HtmlPageRetrieve.FromPath(uriBuilder.Uri.ToString());
        }

        /// <summary>
        /// Returns a html page that contains the bus stations for a particular bus number.
        /// For example, for bus number 11, see http://tursib.ro/traseu/11
        /// </summary>
        /// <param name="busNumber"></param>
        /// <returns></returns>
        public HtmlDocument BusStations(string busNumber)
        {
            UriBuilder uriBuilder = new UriBuilder(tursibUri);
            uriBuilder.Path += "traseu/" + busNumber;
            return HtmlPageRetrieve.FromPath(uriBuilder.Uri.ToString());
        }

        /// <summary>
        /// Returns a html page with bus timetable for the specified bus number and station number.
        /// </summary>
        /// /// <param name="busNumber"></param>
        /// <param name="stationNumber">Stations numbers are from 0 to n as they appear in the html page retrieve by the BusStation method</param>
        /// <returns></returns>
        /// <example>
        /// http://tursib.ro/traseu/11/program?statie=0&dir=dus
        /// first station (0) for bus 11, departure
        /// </example>
        public HtmlDocument BusTimetable(string busNumber, string stationNumber, Direction direction)
        {
            UriBuilder uriBuilder = new UriBuilder(tursibUri);
            uriBuilder.Path += "traseu/" + busNumber + "/program";
            uriBuilder.Query = "statie=" + stationNumber + "&dir=" + direction;

            return HtmlPageRetrieve.FromPath(uriBuilder.Uri.ToString());
        }
    }
}