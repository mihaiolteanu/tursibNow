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
    public class BusStationsViewAdapter : BaseAdapter<Station>
    {
        Activity _context;
        //display all stations for this bus
        Bus _bus;
        //and this direction
        Direction _direction;

        public override Station this[int position]
        {
            get 
            {
                if (_direction == Direction.dus)
                {
                    return _bus.DirectStations.ElementAt(position);
                }
                return _bus.ReverseStations.ElementAt(position);
            }
        }

        public override int Count
        {
            get
            {
                if (_direction == Direction.dus)
                {
                    return _bus.DirectStations.Count();
                }
                return _bus.ReverseStations.Count();
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //reuse an already created view
            View view = convertView;

            //create new view if one does not exist
            if (view == null)
            {
                view = _context.LayoutInflater.Inflate(Resource.Layout.BusStationsItem, null);
            }

            Station station = new Station();
            if (_direction == Direction.dus)
            {
                station = _bus.DirectStations.ElementAt(position);
            }
            if (_direction == Direction.intors)
            {
                station = _bus.ReverseStations.ElementAt(position);
            }
            view.FindViewById<TextView>(Resource.Id.StationNameTextView).Text = station.Name;

            return view;
        }

        public BusStationsViewAdapter(Activity context, Bus bus, Direction direction)
        {
            _context = context;
            _bus = bus;
            _direction = direction;
        }
    }
}