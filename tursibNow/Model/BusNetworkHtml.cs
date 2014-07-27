using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

using tursibNow.HtmlService;

namespace tursibNow.Model
{
    /// <summary>
    /// retrieve the bus network information from html pages
    /// </summary>
    public class BusNetworkHtml : IBusNetwork, IEnumerable<Bus>
    {
        List<Bus> _buses = new List<Bus>();
        public IEnumerable<Bus> Buses 
        {
            get { return _buses; }
        }

        /// <summary>
        /// builds a list of buses from a html file
        /// </summary>
        /// <param name="url"></param>
        public BusNetworkHtml(IHtmlService htmlService)
        {
            //get the names and numbers of buses
            HtmlDocument busOverview = htmlService.BusOverview();

            //get the list of buses in the form
            //<a href="/traseu/1"><strong>Cimitir  - Obi/Viile Sibiului</strong></a>
            //see the BusOverview.htm for an example
            var busesInfo = from node in busOverview.DocumentNode.Descendants("h3")   //bus info is contained after h3 tags
                            where ((node.InnerHtml == "Trasee principale") ||         //avoid h3 tags which are not of interest
                                   (node.InnerHtml == "Trasee secundare") ||
                                   (node.InnerHtml == "Trasee profesionale") ||
                                   (node.InnerHtml == "Trasee turistice"))
                            select node.NextSibling                                   //get the table containing the bus info
                                       .Descendants("a")
                                       .Where(n => n.ParentNode.Attributes["class"].Value == "denumire");

            //transform List<List<HtmlNode> in List<HtmlNode>
            List<HtmlNode> singleList = new List<HtmlNode>();
            foreach (var busInfo in busesInfo)
            {
                foreach (var buss in busInfo)
                {
                    singleList.Add(buss);
                }
            }

            //build an object containing the bus number and name - this still contains html tags which need to be stripped
            //BusNo of the form: /traseu/1
            //BusName of the form: <strong>Cimitir  - Obi/Viile Sibiului</strong>
            var busHtmlObjects = from busList in singleList
                                 let BusNo = busList.Attributes["href"].Value
                                 let BusName = busList.InnerHtml
                                 select new { BusNo, BusName };

            //add each bus to the network
            foreach (var busHtml in busHtmlObjects)
            {
                Bus bus = new Bus();

                //split to get the bus number
                if (busHtml.BusNo.Contains('/'))
                {
                    bus.Number = busHtml.BusNo.Split('/').Last();
                }
                
                //remove <strong> tags to get the bus name
                if (busHtml.BusName.Contains("<strong>") && busHtml.BusName.Contains("</strong>"))
                {
                    bus.Name = busHtml.BusName.Replace("<strong>", "").Replace("</strong>", "");
                }
                
                //new bus added to the network
                _buses.Add(bus);
            }
        }


        #region IEnumerable<Bus> implementation
        public IEnumerator<Bus> GetEnumerator()
        {
            foreach (Bus bus in Buses)
            {
                yield return bus;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
