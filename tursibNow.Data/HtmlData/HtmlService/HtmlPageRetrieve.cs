using System;
using HtmlAgilityPack;

namespace tursibNow.Data
{
    class HtmlPageRetrieve
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