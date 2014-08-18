using System;
using System.IO;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using tursibNow.Core;

namespace tursibNow.AndroidUI
{
    [Activity(Label = "tursibNow", MainLauncher = true)]
    public class BusOverview : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.BusOverview);

            // Set the storage path for the bus info saving/retrieving files
            BusNetwork.StoragePath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "tursibNow");

            var busOverviewListView = FindViewById<ListView>(Resource.Id.BusOverviewListView);
            var adapter = new BusOverviewViewAdapter(this);
            busOverviewListView.Adapter = adapter;

            busOverviewListView.ItemClick += BusClicked;
        }

        protected void BusClicked(object sender, ListView.ItemClickEventArgs e)
        {
            Intent busStationsIntent = new Intent(this, typeof(BusStations));
            busStationsIntent.PutExtra("busNumber", e.Id.ToString());

            StartActivity(busStationsIntent);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.SearchBuses, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch(item.ItemId)
            {
                case Resource.Id.searchBuses:
                    Intent busSearchIntent = new Intent(this, typeof(BusSearch));
                    StartActivity(busSearchIntent);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}

