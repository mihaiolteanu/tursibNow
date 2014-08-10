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
    /// test the correct scrapping of html pages containing the bus network info
    /// </summary>
	[TestFixture]
	public class BusNetworkHtmlTest
	{
        IBusHtmlService htmlService;
        List<Bus> buses;

        public BusNetworkHtmlTest()
        {
            // Get html pages from local storage
            // Create this by hand and add the html files found in this test to shell/emulated/0/tursibNow
            string storagePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            storagePath = Path.Combine(storagePath, "tursibNow");
            
            htmlService = new MockHtmlService(storagePath);
            
            // Build the bus network from local storage
            buses = new BusNetworkHtml(htmlService).Buses as List<Bus>;
        }

		[Test]
        public void GetAllBuses()
		{
            Assert.AreEqual(21, buses.Count);
		}

        // get some random bus numbers and verify they exist in the network with the correct name
        [Test]
        public void GetBusNumberNamePair()
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

        // Test if we get all the existing stations, in both ways (direct and reverse)
        // Test for bus number 11
        [Test]
        public void GetAllStations()
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
        public void GetStationNumberName()
        {
            Bus bus = buses.Find(b => b.Number == "11");
            List<Station> stationsDirect = bus.DirectStations as List<Station>;
            List<Station> stationsReverse = bus.ReverseStations as List<Station>;

            string station0Direct = "CEDONIA".ToLower();
            string station7Direct = "MC DONALD'S".ToLower();
            string station17Direct = "SC CONTINENTAL".ToLower();
            Assert.AreEqual(station0Direct, stationsDirect[0].Name);
            Assert.AreEqual(station7Direct, stationsDirect[7].Name);
            Assert.AreEqual(station17Direct, stationsDirect[16].Name);

            string station0Reverse = "SC CONTINENTAL".ToLower();
            string station4Reverse = "AEROPORT 1".ToLower();
            string station23Reverse = "CEDONIA".ToLower();
            Assert.AreEqual(station0Reverse, stationsReverse[0].Name);
            Assert.AreEqual(station4Reverse, stationsReverse[4].Name);
            Assert.AreEqual(station23Reverse, stationsReverse[23].Name);
        }

        /// <summary>
        /// test if all times entries for a bus station are read (the number of times a bus stops at this station)
        /// we have the station 8 direct (turnisor) timetable for bus 11 in program11_08_turnisor_dus.htm
        /// </summary>
        [Test]
        public void GetAllTimes()
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
}