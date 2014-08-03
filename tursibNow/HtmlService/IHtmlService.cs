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
using tursibNow.Model;

namespace tursibNow.HtmlService
{
    public interface IHtmlService
    {
        /// <summary>
        /// get a html page with all the bus numbers and names (see the tursibHtmlSamples/BusOverview.htm for an example)
        /// </summary>
        HtmlDocument BusOverview();

        /// <summary>
        /// get a html page for a certain bus that contains all the bus stations
        /// </summary>
        /// <param name="busNumber">the bus number for which the html page containing all the stations should be returned</param>
        /// <returns></returns>
        HtmlDocument BusStations(string busNumber);

        /// <summary>
        /// get a html page for a certain bus and a certain station number (station numbers are counted from 0 to the last station)
        /// that contains the bus timetable for that station
        /// </summary>
        /// <param name="busNumber"></param>
        /// <param name="stationNumber"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        HtmlDocument BusTimetable(string busNumber, string stationNumber, Direction direction);
    }
}