using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using HtmlAgilityPack;
using tursibNow.HtmlService;
using tursibNow.Model;

namespace tursibNow.Tests
{
    /// <summary>
    /// test the correct scrapping of html pages containing the bus network info
    /// </summary>
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

        /// <summary>
        /// test if we get all the existing stations, in both ways (direct and reverse)
        /// test for bus number 11
        /// </summary>
        [Test]
        public void BusStation_GetAllStations()
        {
            Bus bus = buses.Find(b => b.Number == "11");

            List<Station> stationsDirect = bus.DirectStations as List<Station>;
            int nStationsDirect = 17; //as seen in traseu11.htm, or at tursib.ro/traseu/11

            List<Station> stationsReverse = bus.ReverseStations as List<Station>;
            int nStationsReverse = 24;

            Assert.AreEqual(nStationsDirect, stationsDirect.Count);
            Assert.AreEqual(nStationsReverse, stationsReverse.Count);
        }

        /// <summary>
        /// test if the station number correlates with the station name
        /// </summary>
        [Test]
        public void BusStation_GetStationNumberName()
        {
            Bus bus = buses.Find(b => b.Number == "11");
            List<Station> stationsDirect = bus.DirectStations as List<Station>;
            List<Station> stationsReverse = bus.ReverseStations as List<Station>;

            string station0Direct = "CEDONIA";
            string station7Direct = "MC DONALD'S";
            string station17Direct = "SC CONTINENTAL";
            Assert.AreEqual(station0Direct, stationsDirect[0].Name);
            Assert.AreEqual(station7Direct, stationsDirect[7].Name);
            Assert.AreEqual(station17Direct, stationsDirect[17].Name);

            string station0Reverse = "SC CONTINENTAL";
            string station4Reverse = "AEROPORT 1";
            string station23Reverse = "CEDONIA";
            Assert.AreEqual(station0Reverse, stationsReverse[0].Name);
            Assert.AreEqual(station4Reverse, stationsReverse[4].Name);
            Assert.AreEqual(station23Reverse, stationsReverse[23].Name);
        }

        /// <summary>
        /// test if all times entries for a bus station are read (the number of times a bus stops at this station)
        /// we have the station 8 direct (turnisor) timetable for bus 11 in program11_08_turnisor_dus.htm
        /// </summary>
        [Test]
        public void BusStationTimetable_GetAllTimes()
        {
            //get bus 11
            Bus bus = buses.Find(b => b.Number == "11");
            
            //get a list of direct stations for bus 11
            List<Station> stationsDirect = bus.DirectStations as List<Station>;

            //get the timetables for station 8 (turnisor) in direct direction
            TimeTable timeTableDirect = stationsDirect[8].TimeTable;

            //the time table for every station consist of different times for monday-friday, saturday and sunday, respectively
            List<DateTime> timeTableWeekDays = timeTableDirect.WeekDays as List<DateTime>;
            List<DateTime> timeTableSaturday = timeTableDirect.Saturday as List<DateTime>;
            List<DateTime> timeTableSunday   = timeTableDirect.Sunday   as List<DateTime>;

            //check the number of entries in the time table
            Assert.AreEqual(43, timeTableWeekDays.Count);
            Assert.AreEqual(21, timeTableSaturday.Count);
            Assert.AreEqual(21, timeTableSunday.Count);
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