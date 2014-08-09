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
    [Activity(Label = "BusTimeTable", Theme = "@android:style/Theme.Light.NoTitleBar.Fullscreen")]
    public class BusTimeTableActivity : Activity
    {
        TimeTable _stationTimetable = new TimeTable();

        protected override void OnCreate(Bundle bundle)
        {
            string busNumber = string.Empty;
            string stationName = string.Empty;

            if (Intent.HasExtra("busNumber") && Intent.HasExtra("busStation") && Intent.HasExtra("busDirection"))
            {
                busNumber = Intent.GetStringExtra("busNumber");
                int busStation = (int)Intent.GetLongExtra("busStation", 0);
                string direction = Intent.GetStringExtra("busDirection");

                
                if (direction == Direction.dus.ToString())
                {
                    //get timetable / direct
                    _stationTimetable =
                        (from bus in BusNetwork.Buses
                        where bus.Number == busNumber
                        select bus.DirectStations.ElementAt(busStation).TimeTable)
                        .First();

                    //get station name / direct
                    stationName = 
                        (from bus in BusNetwork.Buses
                        where bus.Number == busNumber
                        select bus.DirectStations.ElementAt(busStation).Name)
                        .First();
                }

                
                if (direction == Direction.intors.ToString())
                {
                    //get timetable / reverse
                    _stationTimetable =
                        (from bus in BusNetwork.Buses
                         where bus.Number == busNumber
                         select bus.ReverseStations.ElementAt(busStation).TimeTable)
                        .First();

                    //get station name / reverse
                    stationName = 
                        (from bus in BusNetwork.Buses
                        where bus.Number == busNumber
                        select bus.ReverseStations.ElementAt(busStation).Name)
                        .First();
                }
            }

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.BusTimetable);

            FindViewById<TextView>(Resource.Id.StationInfoTextView).Text = 
                busNumber + " - " + stationName;

            TextView weekdaysTextView = FindViewById<TextView>(Resource.Id.StationsWeekdaysTextView);
            TextView saturdayTextView = FindViewById<TextView>(Resource.Id.StationsSaturdayTextView);
            TextView sundayTextView = FindViewById<TextView>(Resource.Id.StationsSundayTextView);

            weekdaysTextView.Text = TimeTableToString(_stationTimetable.WeekDays);
            saturdayTextView.Text = TimeTableToString(_stationTimetable.Saturday);
            sundayTextView.Text = TimeTableToString(_stationTimetable.Sunday);
            
        }

        private string TimeTableToString(IEnumerable<DateTime> timetable)
        {
            string toReturn = string.Empty;

            foreach (DateTime time in timetable)
            {
                toReturn += time.Hour + ":" + time.Minute + ", ";
            }
            return toReturn;
        }
    }

    
}