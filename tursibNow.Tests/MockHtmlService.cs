using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using HtmlAgilityPack;

using tursibNow.Model;
using tursibNow.Data;

namespace tursibNow.Tests
{
    /// <summary>
    /// Mock the html service to retrieve local saved html pages (save in shell/emulated/0/tursibNow)
    /// </summary>
    class MockHtmlService : IBusHtmlService
    {
        //locally stored html files
        string _storagePath;

        public MockHtmlService(string storagePath)
        {
            _storagePath = storagePath;
        }

        /// <summary>
        /// Return a locally stored html page containing the bus overview, as seen on tursib.ro/trasee
        /// </summary>
        /// <returns></returns>
        public HtmlDocument BusOverview()
        {
            //21 different bus numbers/names
            string busOverviewPath = Path.Combine(_storagePath, "trasee.htm");
            HtmlDocument busOverviewHtml = new HtmlDocument();
            busOverviewHtml.Load(busOverviewPath);
            return busOverviewHtml;
        }

        /// <summary>
        /// return a locally stored html page containing the bus station for the given bus number, as seen on tursib.ro/traseu/X, where X is the bus number
        /// </summary>
        /// <param name="busNumber">ignore this</param>
        /// <param name="direction">ignore this</param>
        /// <returns></returns>
        public HtmlDocument BusStations(string busNumber)
        {
            //getting the localy saved html page for bus number 11
            string busStationsPath = Path.Combine(_storagePath, "traseu11.htm");
            HtmlDocument busStationsHtml = new HtmlDocument();
            busStationsHtml.Load(busStationsPath);
            return busStationsHtml;
        }

        /// <summary>
        /// return a locally stored html page containing the bus timetable for a given bus station, as seen on tursib.ro/traseu/X/program?statie=Y&dir=Z, where X is the bus number, Y the station number and Z the direction
        /// </summary>
        /// <param name="busNumber">ignore this</param>
        /// <param name="stationNumber">ignore this</param>
        /// <param name="direction">ignore this</param>
        /// <returns></returns>
        public HtmlDocument BusTimetable(string busNumber, string stationNumber, Direction direction)
        {
            //getting the localy saved html for bus number 11 timetable for station number 8 (turnisor) dus
            string busTimeTable = Path.Combine(_storagePath, "program11_08_turnisor_dus.htm");
            HtmlDocument busTimeTableHtml = new HtmlDocument();
            busTimeTableHtml.Load(busTimeTable);
            return busTimeTableHtml;
        }
    }
}