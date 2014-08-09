using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using tursibNow.Model;

namespace tursibNow.AndroidUI
{
    [Activity(Label = "Bus Stations", Theme = "@android:style/Theme.Light")]
    public class BusStationsActivity : ListActivity
    {
        Bus _bus;

        protected override void OnCreate(Bundle bundle)
        {
            // Extract the bus for which the activity was created
            if (Intent.HasExtra("busNumber"))
            {
                string busNumber = Intent.GetStringExtra("busNumber");
                _bus = BusNetwork.Buses.Where(b => b.Number == busNumber).First();
            }

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            // Labels for each stations list
            Dictionary<string, List<Station>> busStationsLabels = new Dictionary<string, List<Station>>
            {
                {"Direct Routes", _bus.DirectStations.ToList()},
                {"Reverse Routes", _bus.ReverseStations.ToList()}
            };

            var adapter = CreateAdapter(busStationsLabels);
            ListAdapter = adapter;
            
        }

        public override void OnAttachedToWindow() 
        { 
            base.OnAttachedToWindow(); 
            Window.SetTitle(_bus.Number + " - " + _bus.Name); 
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var calculatedId = id;
            // The direct stations label adds one more list item
            calculatedId--;

            Intent stationTimeTableIntent = new Intent(this, typeof(BusTimeTableActivity));
            stationTimeTableIntent.PutExtra("busNumber", _bus.Number);

            int nDirectStations = _bus.DirectStations.ToList().Count;
            // Choose what station direction list to use to extract the time table info
            if (calculatedId <= nDirectStations)
            {
                // The user clicked on a direct station. The id is correct as it is.
                stationTimeTableIntent.PutExtra("busDirection", Direction.dus.ToString());
            }
            else
            {
                // The user clicked on a reverse station.
                stationTimeTableIntent.PutExtra("busDirection", Direction.intors.ToString());

                // This includes an additional label which adds one more list item.
                calculatedId--;

                // Ignore the direct station list items, as we're in the reverse station list items
                calculatedId = calculatedId - nDirectStations;
            }
            stationTimeTableIntent.PutExtra("busStation", calculatedId);
            StartActivity(stationTimeTableIntent);
        }

        SeparatedListAdapter CreateAdapter<T>(Dictionary<string, List<T>> sortedObjects) where T : IHasLabel, IComparable<T>
        {
            var adapter = new SeparatedListAdapter(this);
            foreach (var e in sortedObjects.OrderBy(de => de.Key))
            {
                var label = e.Key;
                var section = e.Value;
                adapter.AddSection(label, new ArrayAdapter<T>(this, Resource.Layout.ListItem, section));
            }
            return adapter;
        }
    }
}