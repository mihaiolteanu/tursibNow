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
    public class BusSearchResultsViewAdapter : BaseAdapter<Bus>
    {
        readonly Activity _context;
        readonly IEnumerable<Tuple<Bus, Station>> _searchResultsBuses;

        public override Bus this[int position]
        {
            get { return _searchResultsBuses.ElementAt(position).Item1; }
        }

        public override int Count
        {
            get { return _searchResultsBuses.Count(); }
        }

        public override long GetItemId(int position)
        {
            int toReturn;
            if (Int32.TryParse(_searchResultsBuses.ElementAt(position).Item1.Number, out toReturn))
            {
                return toReturn;
            }
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //reuse an already created view
            View view = convertView;

            //create new view if one does not exist
            if (view == null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.BusSearchResultsItem, null);
            }

            Bus bus = _searchResultsBuses.ElementAt(position).Item1;
            Station station = _searchResultsBuses.ElementAt(position).Item2;
            view.FindViewById<TextView>(Resource.Id.BusSearchResultsItemBusName).Text = bus.Number + " - " + bus.Name;
            view.FindViewById<TextView>(Resource.Id.BusSearchResultsItemDepartureStationName).Text = ""; //station.Name;
            
            // Add next bus arrivals
            Timing timing = new Timing();
            view.FindViewById<TextView>(Resource.Id.BusSearchResultsItemDepartureTimes).Text =
                timing.NextTimes(station.TimeTable, 5).DateTimeListToString();

            return view;
        }

        public BusSearchResultsViewAdapter(Activity context, IEnumerable<Tuple<Bus, Station>> searchResultsBuses)
        {
            _context = context;
            _searchResultsBuses = searchResultsBuses;
        }
    }
}