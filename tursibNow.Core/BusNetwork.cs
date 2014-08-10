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
    /// Retrieves a bus network from the StoragePath. If the path is empty, a new one is created. 
    /// Uses the path specified by StoragePath to save/update the bus info.
    /// Use:
    /// - Set the StoragePath to a folder location
    /// - Retrieve the bus network with the Buses attribute
    /// </summary>
    public class BusNetwork
    {
        // Storage path for files containing the bus network info
        public static string StoragePath { get; set; }

        private static List<Bus> _buses;
        public static List<Bus> Buses 
        { 
            get 
            {
                if (_buses == null)
                {
                    Update();
                }
                return _buses; 
            } 
        }
        
        static void Update()
        {
            var htmlService = new HtmlServiceTursibRo();
            var busSaveRetrieve = new BusNetworkSaveRetrieve(StoragePath, htmlService);
            _buses = busSaveRetrieve.Buses as List<Bus>;
        }
    }
}