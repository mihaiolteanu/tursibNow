using System;
using System.IO;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace tursibNow
{
    [Activity(Label = "tursibNow", MainLauncher = true, Icon = "@drawable/icon")]
    public class BusOverviewActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //var path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            //path = Path.Combine(path, "tursibNow/testfile.txt");
            //File.WriteAllText(path, "Write this text into a file!");

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.BusOverview);

            ListView busOverviewListView = FindViewById<ListView>(Resource.Id.BusOverviewListView);
            BusOverviewViewAdapter adapter = new BusOverviewViewAdapter(this);
            busOverviewListView.Adapter = adapter;
        }
    }
}

