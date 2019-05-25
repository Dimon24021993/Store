using Store.Parser.Extensions;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Store.Parser.Sites
{
    public static class IntelligentDesign
    {
        public static int Items = 0;
        public static void ParseIntelligentDesign()
        {
            var client = new HttpClient(new HttpClientHandler() { CookieContainer = new CookieContainer() })
            { BaseAddress = new Uri("https://intelligent-design.ru") };

            var document = client.GetDocument("").QuerySelectorAll("");

            var items = client.GetDocument("")
                              ?.QuerySelectorAll("a.mpp_tomoviepage")
                              ?.Select(x => x.GetAttribute("href"))?.ToList();

            if (items.Any())
            {

                foreach (var item in items)
                {
                    ParseIntelligentDesign(ref client, item);
                }
            }

            Console.WriteLine($"Added {Items} movies");
            Console.WriteLine("Done Parse IntelligentDesign!");
        }

        private static void ParseIntelligentDesign(ref HttpClient client, string href)
        {
            try
            {
                var document = client.GetDocument(href);
                var name = document.QuerySelector("h1").TextContent;
                var results = document
                    .QuerySelectorAll("ul.movie_credentials li")
                    .ToDictionary(elem => elem.FirstElementChild.TextContent);

                #region User


                #endregion
            }
            catch (Exception e)
            {
                // ignored
            }
        }


    }
}