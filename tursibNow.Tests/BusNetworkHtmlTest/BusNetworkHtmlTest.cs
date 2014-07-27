using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using HtmlAgilityPack;
using tursibNow.HtmlService;
using tursibNow.Model;

namespace tursibNow.Tests
{
	[TestFixture]
	public class BusNetworkHtmlTest
	{
        IHtmlService htmlService;
        BusNetworkHtml busNetwork;
        List<Bus> buses;

		[SetUp]
		public void Setup ()
		{
            //get html pages from local storage
            htmlService = new MockHtmlService();
            //build the bus network from local storage
            busNetwork = new BusNetworkHtml(htmlService);
            buses = busNetwork.Buses as List<Bus>;
		}

		[TearDown]
		public void TearDown()
		{
            

		}

		[Test]
        public void BusOverview_GetAllBuses()
		{
            Assert.AreEqual(21, buses.Count);
		}

        /// <summary>
        /// get some random bus numbers and verify they exist in the network with the correct name
        /// </summary>
        [Test]
        public void BusOverview_GetBusNumberNamePair()
        {
            Bus bus;

            string busNumber1 = "1";
            string busName1 = "Cimitir  - Obi/Viile Sibiului"; //double space here as it appears in the html file
            bus = buses.Find(b => b.Number == busNumber1);
            Assert.AreEqual(busName1, bus.Name);

            string busNumber11 = "11";
            string busName11 = "Cedonia - SC Continental";
            bus = buses.Find(b => b.Number == busNumber11);
            Assert.AreEqual(busName11, bus.Name);

            string busNumber15 = "15";
            string busName15 = "Valea Aurie - Gara";
            bus = buses.Find(b => b.Number == busNumber15);
            Assert.AreEqual(busName15, bus.Name);

            string busNumber118 = "118";
            string busName118 = "Gusterita - S.C. Continental";
            bus = buses.Find(b => b.Number == busNumber118);
            Assert.AreEqual(busName118, bus.Name);

            string busNumber22 = "22";
            string busName22 = "Sibiu - Paltinis";
            bus = buses.Find(b => b.Number == busNumber22);
            Assert.AreEqual(busName22, bus.Name);
        }


	}

    /// <summary>
    /// retrieve local saved html pages (save in shell/emulated/0/tursibNow)
    /// </summary>
    class MockHtmlService : IHtmlService
    {
        //locally stored html files
        string path;

        /// <summary>
        /// return a locally stored html page containing the bus overview, as seen on tursib.ro/trasee
        /// </summary>
        /// <returns></returns>
        public HtmlDocument BusOverview()
        {
            //21 different bus numbers/names
            string busOverviewPath = Path.Combine(path, "trasee.htm");
            HtmlDocument busOverviewHtml = new HtmlDocument();
            busOverviewHtml.Load(busOverviewPath);
            return busOverviewHtml;
        }

        /// <summary>
        /// return a locally stored html page containing the bus station for the given bus number, as seen on tursib.ro/traseu/X, where X is the bus number
        /// </summary>
        /// <param name="busNumber">ignore this</param>
        /// <returns></returns>
        public HtmlDocument BusStation(int busNumber)
        {
            //getting the localy saved html page for bus number 11
            string busOverviewPath = Path.Combine(path, "traseu11.htm");
            HtmlDocument busOverviewHtml = new HtmlDocument();
            busOverviewHtml.Load(busOverviewPath);
            return busOverviewHtml;
        }

        /// <summary>
        /// return a locally stored html page containing the bus timetable for a given bus station, as seen on tursib.ro/traseu/X/program?statie=Y&dir=Z, where X is the bus number, Y the station number and Z the direction
        /// </summary>
        /// <param name="busNumber">ignore this</param>
        /// <param name="stationNumber">ignore this</param>
        /// <param name="direction">ignore this</param>
        /// <returns></returns>
        public HtmlDocument BusTimetable(int busNumber, int stationNumber, Direction direction)
        {
            //getting the localy saved html for bus number 11 timetable for station number 8 (turnisor) dus
            string busOverviewPath = Path.Combine(path, "program11_08_turnisor_dus.htm");
            HtmlDocument busOverviewHtml = new HtmlDocument();
            busOverviewHtml.Load(busOverviewPath);
            return busOverviewHtml;
        }

        public MockHtmlService()
        {
            //get external address; create this by hand and add the specific html files (shell/emulated/0/tursibNow)
            path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            path = Path.Combine(path, "tursibNow");
        }
    }

}

