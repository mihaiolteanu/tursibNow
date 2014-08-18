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
using tursibNow.Core;

namespace tursibNow.AndroidUI
{
    [Activity(Label = "Bus Search Results")]
    public class BusSearchResults : Activity
    {
        string _departure;
        string _arrival;
        IEnumerable<Tuple<Bus, Station>> _searchResultsBuses;

        protected override void OnCreate(Bundle bundle)
        {
            // Extract the bus for which the activity was created
            if (Intent.HasExtra("departure"))
            {
                _departure = Intent.GetStringExtra("departure");
            }
            if (Intent.HasExtra("arrival"))
            {
                _arrival = Intent.GetStringExtra("arrival");
            }

            _searchResultsBuses = BusNetwork.BusRoutes(_departure, _arrival);

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.BusSearchResults);

            var busSearchViewList = FindViewById<ListView>(Resource.Id.BusSearchResultsListView);
            var adapter = new BusSearchResultsViewAdapter(this, _searchResultsBuses);
            busSearchViewList.Adapter = adapter;
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            Window.SetTitle(_departure + " - " + _arrival);
        }
    }
}