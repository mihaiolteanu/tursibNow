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

using HtmlAgilityPack;

namespace tursibNow.HtmlService
{
    public class HtmlPageRetrieve
    {
        static string _path;
        static string Path { get { return _path; } }
        static HtmlWeb web = new HtmlWeb();

        /// <summary>
        /// retrieves the html page at the specified path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static HtmlDocument FromPath(string path)
        {
            _path = path;
            HtmlDocument doc = new HtmlDocument();
            try
            {
                doc = web.Load(_path);
            }
            catch (Exception e)
            {

            }

            //TBD - check to see if 404 - Page not found was returned 

            return doc;
        }
    }
}