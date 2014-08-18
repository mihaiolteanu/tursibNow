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

using tursibNow.Core;

namespace tursibNow.AndroidUI
{
    [Activity(Label = "Bus Search")]
    public class BusSearch : Activity
    {
        AutoCompleteTextView _busSearchDeparture;
        AutoCompleteTextView _busSearchArrival;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.BusSearch);

            _busSearchDeparture = FindViewById<AutoCompleteTextView>(Resource.Id.BusSearchAutoCompleteDeparture);
            _busSearchArrival = FindViewById<AutoCompleteTextView>(Resource.Id.BusSearchAutoCompleteArrival);

            // Set text view by locale
            FindViewById<TextView>(Resource.Id.BusSearchDeparture).Text = this.GetStringResource(Resource.String.Departure);
            FindViewById<TextView>(Resource.Id.BusSearchArrival).Text = this.GetStringResource(Resource.String.Arrival);

            // Set the autcomplete feature
            var stationNames = BusNetwork.StationNames;
            ArrayAdapter autocompleteAdapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, stationNames);
            _busSearchDeparture.Adapter = autocompleteAdapter;
            _busSearchArrival.Adapter = autocompleteAdapter;

            // The search button
            var busSearchButton = FindViewById<Button>(Resource.Id.BusSearchButton);
            busSearchButton.Text = this.GetStringResource(Resource.String.Search);
            busSearchButton.Click += BusSearchButtonClick;
        }

        private void BusSearchButtonClick(object sender, EventArgs e)
        {
            // Get user input data
            string departure = _busSearchDeparture.Text;
            string arrival = _busSearchArrival.Text;

            // Validate user data
            if ((departure == null || departure.Length < 3) || (arrival == null || arrival.Length < 3))
            {
                this.AlertDialogCreate
                    (title: this.GetStringResource(Resource.String.InvalidStationTitle),
                    message: this.GetStringResource(Resource.String.InvalidStationMessage));
                return;
            }

            if (!BusNetwork.ContainsStationName(departure))
            {
                this.AlertDialogCreate
                    (title: this.GetStringResource(Resource.String.InvalidStationTitle),
                    message: this.GetStringResource(Resource.String.DepartureMessage));
                return;
            }

            if (!BusNetwork.ContainsStationName(arrival))
            {
                this.AlertDialogCreate
                    (title: this.GetStringResource(Resource.String.InvalidStationTitle),
                    message: this.GetStringResource(Resource.String.ArrivalMessage));
                return;
            }

            // All is good
            Intent intentResults = new Intent(this, typeof(BusSearchResults));
            intentResults.PutExtra("departure", departure);
            intentResults.PutExtra("arrival", arrival);

            StartActivity(intentResults);
        }


        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            Window.SetTitle(this.GetStringResource(Resource.String.BusSearch));
        }
    }
}