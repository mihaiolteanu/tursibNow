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
    public class BusOverviewActivity : Activity
    {
        ListView _busOverviewListView;
        BusOverviewViewAdapter _adapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.BusOverview);

            // Set the storage path for the bus info saving/retrieving files
            BusNetwork.StoragePath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "tursibNow");

            _busOverviewListView = FindViewById<ListView>(Resource.Id.BusOverviewListView);
            _adapter = new BusOverviewViewAdapter(this);
            _busOverviewListView.Adapter = _adapter;

            _busOverviewListView.ItemClick += BusClicked;
        }

        protected void BusClicked(object sender, ListView.ItemClickEventArgs e)
        {
            Intent busStationsIntent = new Intent(this, typeof(BusStationsActivity));
            busStationsIntent.PutExtra("busNumber", e.Id.ToString());

            StartActivity(busStationsIntent);
        }
    }
}

