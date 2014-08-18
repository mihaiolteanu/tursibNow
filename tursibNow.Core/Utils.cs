using System;
using System.Collections.Generic;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Android.Content;
using Android.App;

namespace tursibNow.Core
{
    public static class Utils
    {
        /// <summary>
        /// Returns a list of DateTime objects as a string of the form hour:minute[, hour:minute]
        /// </summary>
        /// <param name="timeTable"></param>
        /// <returns></returns>
        public static string DateTimeListToString(this IEnumerable<DateTime> timeTable)
        {
            string toReturn = string.Empty;

            bool first = true;
            foreach (var time in timeTable)
            {
                // Avoid "," at the end of the returned string
                if (first)
                {
                    toReturn += DateTimeToString(time);
                    first = false;
                    continue; // Next element
                }
                toReturn += ", " + DateTimeToString(time);
            }
            return toReturn;
        }

        public static string DateTimeToString(DateTime dateTime)
        {
            return dateTime.Hour + ":" + dateTime.Minute.ToString("D2");
        }

        /// <summary>
        /// Performs an object deep copy
        /// See "How do you do a deep copy an object in .Net" - SO 129389
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static void AlertDialogCreate(this Context context, string title, string message)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            AlertDialog alertDialog = builder.Create();

            alertDialog.SetTitle(title);
            alertDialog.SetMessage(message);
            alertDialog.SetButton("OK", (s, ev) => { });
            alertDialog.Show();
        }
    }
}