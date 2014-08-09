using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using tursibNow.Model;

namespace tursibNow.Data
{
    /// <summary>
    /// Retrieves the bus network info from the storage path, or saves a new bus network to the storage path 
    /// if none exists, by searching in an external http resource
    /// </summary>
    public class BusNetworkSaveRetrieve
    {
        string _storagePath;
        private IEnumerable<Bus> _buses = new List<Bus>();

        public IEnumerable<Bus> Buses
        {
            get
            {
                // Buses list is empty
                if (!_buses.Any())
                {
                    // Retrieve the buses from json file from the location specified in the constructor.
                    _buses = BusNetworkJson();
                }
                return _buses;
            }
        }

        // Make possible to inject a new htmlService (for tests, for example)
        private IHtmlService _htmlService;
        public IHtmlService HtmlService
        {
            get
            {
                if (_htmlService == null)
                {
                    // Default service if nothing else is set
                    _htmlService = new HtmlServiceTursibRo();
                }
                return _htmlService;
            }
            set { _htmlService = value; }
        }

        /// <summary>
        /// Retrieves a tursib bus network from a given storage path
        /// </summary>
        /// <param name="storagePath">Path to where json files containing the tursib bus network info are located</param>
        public BusNetworkSaveRetrieve(string storagePath)
        {
            _storagePath = storagePath;
        }

        /// <summary>
        /// Retrieves the bus network info from json storage files
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Bus> BusNetworkJson()
        {
            IEnumerable<Bus> buses = new BusNetworkJson(_storagePath).Buses;
            if (!buses.Any())
            {
                // There are no Json files with bus network info available at the specified location.
                buses = BusNetworkFromHttp();
            }
            return buses;
        }

        /// <summary>
        /// Creates new Json files from html and returns the bus network just saved.
        /// </summary>
        private IEnumerable<Bus> BusNetworkFromHttp()
        {            
            // Get a new list of buses from tursib.ro.
            IEnumerable<Bus> newBuses = new BusNetworkHtml(HtmlService).Buses;

            if (newBuses.Any())
            {
                // Save the new list of buses as json files.
                BusNetworkJson busNetworkJson = new BusNetworkJson(_storagePath);
                busNetworkJson.SaveBusNetwork(newBuses);
            }

            return newBuses;
        }
    }
}