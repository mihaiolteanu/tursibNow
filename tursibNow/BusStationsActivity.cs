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

using tursibNow.Model;

namespace tursibNow
{
    [Activity(Label = "Bus Stations", Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen")]
    public class BusStationsActivity : Activity
    {
        ListView _busStationsDirectListView;
        BusStationsViewAdapter _adapterDirect;
        ListView _busStationsReverseListView;
        BusStationsViewAdapter _adapterReverse;

        TextView _busNo;
        TextView _busName;

        Bus bus;

        protected override void OnCreate(Bundle bundle)
        {
            if (Intent.HasExtra("busNumber"))
            {
                string busNumber = Intent.GetStringExtra("busNumber");
                bus = BusNetwork.Buses.Where(b => b.Number == busNumber).First();
            }

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.BusStations);

            //display the bus number and name
            _busNo = FindViewById<TextView>(Resource.Id.BusNoTextView);
            _busName = FindViewById<TextView>(Resource.Id.BusNameTextView);
            _busNo.Text = bus.Number;
            _busName.Text = bus.Name;

            //display all the direct stations for this bus
            _busStationsDirectListView = FindViewById<ListView>(Resource.Id.BusStationsDirectListView);
            _adapterDirect = new BusStationsViewAdapter(this, bus, Direction.dus);
            _busStationsDirectListView.Adapter = _adapterDirect;

            //display all the reverse stations for this bus
            _busStationsReverseListView = FindViewById<ListView>(Resource.Id.BusStationsReverseListView);
            _adapterReverse = new BusStationsViewAdapter(this, bus, Direction.intors);
            _busStationsReverseListView.Adapter = _adapterReverse;
        }
    }
}