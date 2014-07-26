using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

using tursibNow.HtmlService;

namespace tursibNow.Model
{
    //tursib has more than one bus on the road
    public class BusNetworkHtml : IBusNetwork, IEnumerable<Bus>
    {
        List<Bus> buses;
        public IEnumerable<Bus> Buses 
        {
            get { return buses; }
        }

        /// <summary>
        /// builds a list of buses from a html file
        /// </summary>
        /// <param name="url"></param>
        public BusNetworkHtml(IHtmlService htmlService)
        {
            //get the names and numbers of buses
            HtmlDocument busOverview = htmlService.BusOverview;

            //get the <table> nodes where the bus info is stored
            var busInfoTables = from node in busOverview.DocumentNode.Descendants("h3")     //bus info is contained after h3 tags
                                where ((node.InnerHtml == "Trasee principale") ||   //avoid h3 tags which are not of interest
                                        (node.InnerHtml == "Trasee secundare") ||
                                        (node.InnerHtml == "Trasee profesionale") ||
                                        (node.InnerHtml == "Trasee turistice"))
                                select node.NextSibling;                            //get tables

            //object containing the bus number and name
            var BussInfo = from tr in busInfoTables
                           let BusNo = tr.Descendants("a").Where(node => node.ParentNode.Attributes["class"].Value == "cod")
                           let BusName = tr.Descendants("a").Where(node => node.ParentNode.Attributes["class"].Value == "denumire")
                           select new { BusNo, BusName };

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
