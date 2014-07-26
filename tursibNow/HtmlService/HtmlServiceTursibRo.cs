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
    /// <summary>
    /// implements the retrieval of html web pages containing bus info from www.tursib.ro
    /// </summary>
    public class HtmlServiceTursibRo : IHtmlService
    {
        //html page retriever
        HtmlWeb web;
        //tursib official web page
        Uri tursibUri = new Uri("http://tursib.ro/");

        public HtmlDocument BusOverview
        {
            get 
            {
                UriBuilder uriBuilder = new UriBuilder(tursibUri);
                uriBuilder.Path += "/trasee";             
                return web.Load(uriBuilder.Uri.ToString());
            }
        }

        /// <summary>
        /// provides a html pages that contains the bus stations for a particular bus number
        /// for example, for bus number 11, see http://tursib.ro/traseu/11
        /// </summary>
        /// <param name="busNumber"></param>
        /// <returns></returns>
        public HtmlDocument BusStation(int busNumber)
        {
            UriBuilder uriBuilder = new UriBuilder(tursibUri);
            uriBuilder.Path += "traseu/";
            uriBuilder.Path += busNumber;

            return web.Load(uriBuilder.Uri.ToString());
        }

        /// <summary>
        /// provides a bust timetable for the specified station number
        /// </summary>
        /// <param name="stationNumber">stations numbers are from 0 to n as they appear in the html page retrieve by the BusStation method</param>
        /// <returns></returns>
        public HtmlDocument BusTimetable(int stationNumber)
        {
            UriBuilder uriBuilder = new UriBuilder(tursibUri);
            throw new NotImplementedException();
        }
    }
}