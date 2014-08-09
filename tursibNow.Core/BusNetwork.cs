using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using tursibNow.Model;
using tursibNow.Data;

namespace tursibNow.Core
{
    public class BusNetwork
    {
        // Storage path for files containing the bus network info
        static string _storagePath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "tursibNow");
        
        // Retrieve buses info from the storage path
        public static List<Bus> Buses = new BusNetworkSaveRetrieve(_storagePath).Buses as List<Bus>;

        static BusNetwork()
        {
            
        }
    }
}