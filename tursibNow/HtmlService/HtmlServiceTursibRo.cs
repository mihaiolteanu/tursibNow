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

using HtmlAgilityPack;

namespace tursibNow.HtmlService
{
    /// <summary>
    /// implements the retrieval of html web pages containing bus info from www.tursib.ro
    /// </summary>
    public class HtmlServiceTursibRo : IHtmlService
    {
        //tursib official web page - starting place for all html pages to be retrieved
        Uri tursibUri = new Uri("http://tursib.ro/");

        /// <summary>
        /// provides a html page with all the buses available from tursib
        /// </summary>
        public HtmlDocument BusOverview()
        {
            UriBuilder uriBuilder = new UriBuilder(tursibUri);
            uriBuilder.Path += "/trasee";
            return HtmlPageRetrieve.Path(uriBuilder.Uri.ToString());
        }

        /// <summary>
        /// provides a html page that contains the bus stations for a particular bus number
        /// for example, for bus number 11, see http://tursib.ro/traseu/11
        /// </summary>
        /// <param name="busNumber"></param>
        /// <returns></returns>
        public HtmlDocument BusStation(int busNumber)
        {
            UriBuilder uriBuilder = new UriBuilder(tursibUri);
            uriBuilder.Path += "traseu/" + busNumber;

            return HtmlPageRetrieve.Path(uriBuilder.Uri.ToString());
        }

        /// <summary>
        /// provides a html page with bus timetable for the specified bus number and station number
        /// </summary>
        /// /// <param name="busNumber"></param>
        /// <param name="stationNumber">stations numbers are from 0 to n as they appear in the html page retrieve by the BusStation method</param>
        /// <returns></returns>
        /// <example>
        /// http://tursib.ro/traseu/11/program?statie=0&dir=dus
        /// first station (0) for bus 11, departure
        /// </example>
        public HtmlDocument BusTimetable(int busNumber, int stationNumber, Direction direction)
        {
            UriBuilder uriBuilder = new UriBuilder(tursibUri);
            uriBuilder.Path += "traseu/" + busNumber + "/program";
            uriBuilder.Query = "statie=" + stationNumber + "&dir=" + direction;

            return HtmlPageRetrieve.Path(uriBuilder.Uri.ToString());
        }
    }

    /// <summary>
    /// same station can be an arrival or departure point for a bus
    /// </summary>
    public enum Direction
    {
        direct, //dus (romanian)
        reverse //intors (romanian)
    }
}