using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using tursibNow.Model;
using tursibNow.Data;

namespace tursibNow.Core
{
    /// <summary>
    /// Retrieves the bus network info from the storage path, or saves a new bus network to the storage path 
    /// if none exists, by searching in an external http resource, web server, etc.
    /// </summary>
    public class BusNetworkSaveRetrieve
    {
        string _storagePath;
        IBusHtmlService _htmlService;
        IEnumerable<Bus> _buses;
        public IEnumerable<Bus> Buses { get { return _buses; } }

        /// <summary>
        /// Retrieves a tursib bus network from local storage or other resource if local storage not available
        /// Saves the succesfully retrieve bus network to local storage in case it was empty
        /// </summary>
        /// <param name="storagePath">Path to where json files containing the tursib bus network info are located</param>
        public BusNetworkSaveRetrieve(string storagePath, IBusHtmlService htmlService)
        {
            _storagePath = storagePath;
            _htmlService = htmlService;
            // Retrieve the buses from json file from the location specified in the constructor.
            _buses = BusNetworkJson();

            // No bus info found in json files
            if (!_buses.Any())
            {
                // Try to update the bus info from other sources
                _buses = BusNetworkFromHtml(_htmlService);

                // If succesfully retrieved a list of buses from html 
                if (_buses.Any())
                {
                    // Save the new list of buses as json files.
                    var json = new BusNetworkJson(_storagePath);
                    json.Save(_buses);
                }
            }
        }

        // Retrieves the bus network info from json storage files
        private IEnumerable<Bus> BusNetworkJson()
        {
            return new BusNetworkJson(_storagePath).Buses;
        }

        // Creates new Json files from html and returns the bus network just saved.
        private IEnumerable<Bus> BusNetworkFromHtml(IBusHtmlService htmlService)
        {            
            // Get a new list of buses from tursib.ro.
            return new BusNetworkHtml(htmlService).Buses;
        }
    }
}