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
    /// tests the correct saving and retrieving of bus network info to and from JSON files
    /// </summary>
    [TestFixture]
    public class BusNetworkJSONTest
    {
        string storagePath;
        BusNetworkJSON busNetworkJSON;
        List<Bus> buses;

        public BusNetworkJSONTest()
        {
            storagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            busNetworkJSON = new BusNetworkJSON(storagePath);

            //create a network with two buses
            buses = new List<Bus>
            {
                new Bus()
                {
                    Name = "Cedonia - SC Continental",
                    Number = "11",
                    DirectStations = new List<Station>
                    {
                        new Station
                        {
                            Name = "Turnisor",
                            TimeTable = new TimeTable()
                            {
                                WeekDays = new List<DateTime>
                                {
                                    new DateTime(year: 2014, month: 1, day: 1,  hour: 6, minute: 35, second: 1),
                                },
                                Saturday = new List<DateTime>
                                {
                                    new DateTime(year: 2014, month: 1, day: 1,  hour: 16, minute: 00, second: 1),
                                },
                                Sunday = new List<DateTime>
                                {
                                    new DateTime(year: 2014, month: 1, day: 1,  hour: 20, minute: 59, second: 1),
                                }
                                
                            }
                        }
                    }
                },

                new Bus()
                {
                    Name = "Strand - Gara",
                    Number = "17",
                    DirectStations = new List<Station>
                    {
                        new Station
                        {
                            Name = "Strand",
                            TimeTable = new TimeTable()
                            {
                                WeekDays = new List<DateTime>
                                {
                                    new DateTime(year: 2014, month: 1, day: 1,  hour: 5, minute: 30, second: 1),
                                },
                                Saturday = new List<DateTime>
                                {
                                    new DateTime(year: 2014, month: 1, day: 1,  hour: 08, minute: 10, second: 1),
                                },
                                Sunday = new List<DateTime>
                                {
                                    new DateTime(year: 2014, month: 1, day: 1,  hour: 15, minute: 50, second: 1),
                                }
                                
                            }
                        }
                    }
                }
            };
        }

        [SetUp]
        public void Setup()
        {
            //clear any existing json files
            foreach (string filename in Directory.EnumerateFiles(storagePath, "*.json"))
            {
                File.Delete(filename);
            }
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void SaveBusNetwork_CorrectNumberOfFiles()
        {
            busNetworkJSON.SaveBusNetwork(buses);

            //count the number of save json files
            int jsonFiles = 0;
            foreach (string filename in Directory.EnumerateFiles(storagePath, "*.json"))
            {
                jsonFiles++;
            }

            Assert.AreEqual(2, jsonFiles);
        }

        [Test]
        public void RetrieveBusNetwork_CorrectNumberOfBuses()
        {
            busNetworkJSON.SaveBusNetwork(buses);
            List<Bus> retrievedBuses = busNetworkJSON.Buses as List<Bus>;

            Assert.AreEqual(2, retrievedBuses.Count);
        }

        [Test]
        public void RetrieveBusNetwork_CorrectContents()
        {
            busNetworkJSON.SaveBusNetwork(buses);
            List<Bus> retrievedBuses = busNetworkJSON.Buses as List<Bus>;

            Bus bus1 = retrievedBuses.Find(b => b.Name == "Cedonia - SC Continental");
            Bus bus2 = retrievedBuses.Find(b => b.Name == "Strand - Gara");

            Assert.IsNotNull(bus1);
            Assert.IsNotNull(bus2);

            Assert.AreEqual("11", bus1.Number);
            Assert.AreEqual("17", bus2.Number);
        }
    }
}