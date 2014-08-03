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

        Bus _bus;

        protected override void OnCreate(Bundle bundle)
        {
            if (Intent.HasExtra("busNumber"))
            {
                string busNumber = Intent.GetStringExtra("busNumber");
                _bus = BusNetwork.Buses.Where(b => b.Number == busNumber).First();
            }

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.BusStations);

            //display the bus number and name
            FindViewById<TextView>(Resource.Id.BusNumberAndNameTextView).Text = _bus.Number + " - " + _bus.Name;

            //display all the direct stations for this bus
            _busStationsDirectListView = FindViewById<ListView>(Resource.Id.BusStationsDirectListView);
            _adapterDirect = new BusStationsViewAdapter(this, _bus, Direction.dus);
            _busStationsDirectListView.Adapter = _adapterDirect;

            //display all the reverse stations for this bus
            _busStationsReverseListView = FindViewById<ListView>(Resource.Id.BusStationsReverseListView);
            _adapterReverse = new BusStationsViewAdapter(this, _bus, Direction.intors);
            _busStationsReverseListView.Adapter = _adapterReverse;

            _busStationsDirectListView.ItemClick += StationDirectClicked;
            _busStationsReverseListView.ItemClick += StationReverseClicked;

        }

        protected void StationDirectClicked(object sender, ListView.ItemClickEventArgs e)
        {
            Intent stationTimeTableIntent = new Intent(this, typeof(BusTimeTableActivity));
            stationTimeTableIntent.PutExtra("busNumber", _bus.Number);
            stationTimeTableIntent.PutExtra("busStation", e.Id);
            stationTimeTableIntent.PutExtra("busDirection", Direction.dus.ToString());

            StartActivity(stationTimeTableIntent);
        }

        protected void StationReverseClicked(object sender, ListView.ItemClickEventArgs e)
        {
            Intent stationTimeTableIntent = new Intent(this, typeof(BusTimeTableActivity));
            stationTimeTableIntent.PutExtra("busNumber", _bus.Number);
            stationTimeTableIntent.PutExtra("busStation", e.Id);
            stationTimeTableIntent.PutExtra("busDirection", Direction.intors.ToString());

            StartActivity(stationTimeTableIntent);
        }
    }
}