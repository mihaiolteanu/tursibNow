using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using tursibNow.Model;

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
                    toReturn += time.Hour + ":" + time.Minute.ToString("D2");
                    first = false;
                    continue; // Next element
                }
                toReturn += ", " + time.Hour + ":" + time.Minute.ToString("D2");
            }
            return toReturn;
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
    }
}