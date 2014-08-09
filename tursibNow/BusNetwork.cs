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

using tursibNow.Model;
using tursibNow.Data;

namespace tursibNow.AndroidUI
{
    public class BusNetwork
    {
        static string _storagePath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "tursibNow");
        static BusNetworkMaintainer _maintainer = new BusNetworkMaintainer(_storagePath);
        public static List<Bus> Buses = _maintainer.Buses as List<Bus>;

        static BusNetwork()
        {

        }
    }
}