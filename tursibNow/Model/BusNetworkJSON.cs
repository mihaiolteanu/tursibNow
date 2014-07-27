using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Newtonsoft.Json;

namespace tursibNow.Model
{
    /// <summary>
    /// save and retrieve the bus network information to and from JSON files
    /// </summary>
    public class BusNetworkJSON : IBusNetwork
    {
        string _storagePath;
        List<Bus> _buses = new List<Bus>();
        public IEnumerable<Bus> Buses
        {
            get
            {
                //update the bus network list if it's empty
                if (_buses.Count == 0)
                {
                    UpdateBusNetwork();
                }
                return _buses;
            }
        }

        public BusNetworkJSON(string storagePath)
        {
            _storagePath = storagePath;

            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        /// <summary>
        /// saves each bus in a bus network to a JSON file
        /// </summary>
        /// <param name="busNetwork"></param>
        public void SaveBusNetwork (IEnumerable<Bus> busNetwork)
        {
            foreach(Bus bus in busNetwork)
            {
                string busString = JsonConvert.SerializeObject(bus);

                File.WriteAllText(GetFileName(bus.Number), busString);
            }   
        }

        /// <summary>
        /// updates the bus network list from saved json files
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
        /// each bus has a unique json file, based on the bus number
        /// </summary>
        /// <param name="id"></param>
        /// <returns>path to where the json file for this bus is located</returns>
        private string GetFileName(string busNumber)
        {
            return Path.Combine(_storagePath, "bus_" + busNumber + ".json");
        }
    }
}