using System;
using System.IO;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using tursibNow.Model;

namespace tursibNow
{
    [Activity(Label = "tursibNow", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen")]
    public class BusOverviewActivity : Activity
    {
        ListView _busOverviewListView;
        BusOverviewViewAdapter _adapter;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.BusOverview);

            _busOverviewListView = FindViewById<ListView>(Resource.Id.BusOverviewListView);
            _adapter = new BusOverviewViewAdapter(this);
            _busOverviewListView.Adapter = _adapter;

            _busOverviewListView.ItemClick += BusClicked;
        }

        protected void BusClicked(object sender, ListView.ItemClickEventArgs e)
        {
            //Bus bus = BusNetwork.Buses.Find(b => b.Number == e.Id.ToString());

            Intent busStationsIntent = new Intent(this, typeof(BusStationsActivity));
            busStationsIntent.PutExtra("busNumber", e.Id.ToString());

            StartActivity(busStationsIntent);
        }
    }
}

