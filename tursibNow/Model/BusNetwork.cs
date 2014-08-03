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

using tursibNow.HtmlService;

namespace tursibNow.Model
{
    /// <summary>
    /// retrieves the tursib bus network
    /// </summary>
    public class BusNetwork
    {
        static string path = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "tursibNow");
        
        static IHtmlService htmlService = new HtmlServiceTursibRo();
        static BusNetworkJSON busNetworkJson = new BusNetworkJSON(path);
        
        public static List<Bus> Buses = busNetworkJson.Buses as List<Bus>;

        static BusNetwork()
        {
            //busNetworkJson.SaveBusNetwork(new BusNetworkHtml(htmlService).Buses);
        }
        
        //public static IEnumerable<Bus> Buses = new BusNetworkHtml(htmlService).Buses;
    }
}