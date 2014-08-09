using System;
using System.Collections.Generic;
using System.Linq;

namespace tursibNow.Data
{
    static class HtmlServiceUtil
    {
        /// <summary>
        /// Returns all strings from a list that can be evaluated as a datetime object as a datetime list, ignores the rest.
        /// </summary>
        /// <param name="dateTimeStr"></param>
        /// <returns></returns>
        public static List<DateTime> StringsToDateTimes(List<string> dateTimeStr)
        {
            List<DateTime> dateTimeList = new List<DateTime>();

            foreach (string timeStr in dateTimeStr)
            {
                DateTime toAdd;
                if (DateTime.TryParse(timeStr, out toAdd))
                {
                    dateTimeList.Add(toAdd);
                }
            }
            return dateTimeList;
        }

        /// <summary>
        /// Extract bus number from href="/traseu/X", where X is the bus number.
        /// </summary>
        /// <param name="href">string containing "/traseu/X"</param>
        /// <returns>X</returns>
        public static string ExtractBusNumber(string href)
        {
            if (href.Contains('/'))
            {
                return href.Split('/').Last();
            }
            return string.Empty;
        }
    }
}