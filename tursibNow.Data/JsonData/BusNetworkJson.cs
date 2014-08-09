using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using tursibNow.Model;

namespace tursibNow.Data
{
    /// <summary>
    /// Save and retrieve the bus network information to and from Json files
    /// </summary>
    public class BusNetworkJson : IBusNetwork
    {
        string _storagePath;
        List<Bus> _buses = new List<Bus>();

        public IEnumerable<Bus> Buses
        {
            get
            {
                // Update the bus network list if it's empty.
                if (_buses.Count == 0)
                {
                    UpdateBusNetwork();
                }
                return _buses;
            }
        }

        /// <summary>
        /// Saves and retrieves a bus network (bus list) from json files
        /// </summary>
        /// <param name="storagePath">Path to where json files containing bus info are located</param>
        public BusNetworkJson(string storagePath)
        {
            _storagePath = storagePath;

            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        /// <summary>
        /// Saves each bus in a bus network to a Json file
        /// </summary>
        /// <param name="busNetwork">Bus network to be save to Json files</param>
        public void SaveBusNetwork (IEnumerable<Bus> busNetwork)
        {
            foreach(Bus bus in busNetwork)
            {
                string busString = JsonConvert.SerializeObject(bus);

                File.WriteAllText(GetFileName(bus.Number), busString);
            }   
        }

        /// <summary>
        /// Updates the bus network list from saved json files
        /// </summary>
        private void UpdateBusNetwork()
        {
            _buses.Clear();
            string[] filenames = Directory.GetFiles(_storagePath, "*.json");

            foreach (string filename in filenames)
            {
                string busString = File.ReadAllText(filename);
                Bus bus = JsonConvert.DeserializeObject<Bus>(busString);
                _buses.Add(bus);
            }
        }

        /// <summary>
        /// Each bus has a unique json file, based on the bus number.
        /// </summary>
        /// <returns>Path to where the json file for this bus is located</returns>
        private string GetFileName(string busNumber)
        {
            // make all files have the bus number with 3 digits
            if (busNumber.Length == 2)
            {
                busNumber = "0" + busNumber;
            }
            if (busNumber.Length == 1)
            {
                busNumber = "00" + busNumber;
            }

            return Path.Combine(_storagePath, "bus_" + busNumber + ".json");
        }
    }
}