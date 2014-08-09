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

namespace tursibNow.AndroidUI
{
    public class BusOverviewViewAdapter : BaseAdapter<Bus>
    {
        readonly Activity _context;

        public override Bus this[int position]
        {
            get { return BusNetwork.Buses.ElementAt(position); }
        }

        public override int Count
        {
            get { return BusNetwork.Buses.Count(); }
        }

        public override long GetItemId(int position)
        {
            int toReturn;
            if (Int32.TryParse(BusNetwork.Buses.ElementAt(position).Number, out toReturn))
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
                view = _context.LayoutInflater.Inflate(Resource.Layout.BusOverviewItem, null);
            }

            Bus bus = BusNetwork.Buses.ElementAt(position);
            view.FindViewById<TextView>(Resource.Id.BusNoTextView).Text = bus.Number;
            view.FindViewById<TextView>(Resource.Id.BusNameTextView).Text = bus.Name;

            return view;
        }

        public BusOverviewViewAdapter(Activity context)
        {
            _context = context;
        }
    }
}