using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using tursibNow.Model;

namespace tursibNow.Data
{
    /// <summary>
    /// Save and retrieve the bus network information to and from Json files
    /// </summary>
    public class BusNetworkJson
    {
        string _storagePath;
        List<Bus> _buses = new List<Bus>();
        public IEnumerable<Bus> Buses {get {return _buses; } }

        /// <summary>
        /// Saves and retrieves a bus network (bus list) from json files
        /// </summary>
        /// <param name="storagePath">Path to where json files containing bus info are located</param>
        public BusNetworkJson(string storagePath)
        {
            _storagePath = storagePath;
            Android.Util.Log.Info("storagePat", _storagePath);
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
            else
            {
                Update();
            }
        }

        /// <summary>
        /// Saves each bus in a bus network to a Json file
        /// </summary>
        /// <param name="busNetwork">Bus network to be save to Json files</param>
        public void Save(IEnumerable<Bus> busNetwork)
        {
            foreach(Bus bus in busNetwork)
            {
                string busString = JsonConvert.SerializeObject(bus);
                File.WriteAllText(GetFileName(bus.Number), busString);
            }   
        }

        // Updates the bus network list from saved json files.
        private void Update()
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

        // Each bus has a unique json file, based on the bus number.
        // Returns the path to where the json file for this bus is located.
        private string GetFileName(string busNumber)
        {
            // Make all files have the bus number with 3 digits
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