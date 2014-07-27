using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        //currently retrieves the bus network from web, every time the app is started
        //TBD retrieve the bus network from locally stored/serialized JSON files
        //TBD update the locallay stored JSON files from web only on demand
        static IHtmlService htmlService = new HtmlServiceTursibRo();
        public static IEnumerable<Bus> Buses = new BusNetworkHtml(htmlService).Buses;
    }
}