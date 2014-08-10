using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using tursibNow.Core;
using tursibNow.Model;

namespace tursibNow.AndroidUI
{
    [Activity(Label = "BusTimeTable")]
    public class BusTimeTableActivity : Activity
    {
        TimeTable _stationTimetable = new TimeTable();
        string _busNumber = string.Empty;
        string _stationName = string.Empty;

        protected override void OnCreate(Bundle bundle)
        {
            if (Intent.HasExtra("busNumber") && Intent.HasExtra("busStation") && Intent.HasExtra("busDirection"))
            {
                _busNumber = Intent.GetStringExtra("busNumber");
                int busStation = (int)Intent.GetLongExtra("busStation", 0);
                string direction = Intent.GetStringExtra("busDirection");

                GetStationInfo(busStation, direction, out _stationName, out _stationTimetable);
            }

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.BusTimetable);

            FindViewById<TextView>(Resource.Id.StationsWeekdaysTextView).Text =
                _stationTimetable.WeekDays.DateTimeListToString();
            FindViewById<TextView>(Resource.Id.StationsSaturdayTextView).Text =
                _stationTimetable.Saturday.DateTimeListToString();
            FindViewById<TextView>(Resource.Id.StationsSundayTextView).Text =
                _stationTimetable.Sunday.DateTimeListToString();
        }

        /// <summary>
        /// Get the station name and time table based on the station number and direction
        /// </summary>
        /// <param name="busStation"></param>
        /// <param name="direction"></param>
        /// <param name="stationName"></param>
        /// <param name="timeTable"></param>
        public void GetStationInfo(int busStation, string direction, out string stationName, out TimeTable timeTable)
        {
            stationName = (from bus in BusNetwork.Buses
                           where bus.Number == _busNumber
                           select direction == Direction.dus.ToString() ? 
                                bus.DirectStations.ElementAt(busStation).Name : 
                                bus.ReverseStations.ElementAt(busStation).Name
                          ).First();

            timeTable = (from bus in BusNetwork.Buses
                         where bus.Number == _busNumber
                         select direction == Direction.dus.ToString() ? 
                                bus.DirectStations.ElementAt(busStation).TimeTable :
                                bus.ReverseStations.ElementAt(busStation).TimeTable 
                        ).First();
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            Window.SetTitle(_busNumber + " - " + _stationName.ToUpper());
        }
    }
}