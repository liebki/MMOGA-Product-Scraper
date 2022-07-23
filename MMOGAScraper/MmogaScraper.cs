using HtmlAgilityPack;

namespace MMOGAScraper
{
    public static class MmogaScraper
    {
        public static List<Product> DeeperSearch(string query)
        {
            if (query.Length < 3)
            {
                ThrowTooShortError();
            }
            List<Product> ProductList = new();
            List<string> urlListe = new();

            using (HttpClient client = new())
            {
                string result = QueryLinkGetResult(query, client);
                if (!object.Equals(result, null))
                {
                    HtmlDocument doc = new();
                    doc.LoadHtml(result);
                    int a = 0;
                    foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='searchCont']"))
                    {
                        a++;
                        foreach (HtmlNode node2 in doc.DocumentNode.SelectNodes("/html/body/div[2]/div/div[2]/div[2]/div[" + a + "]/ul/li[2]/a"))
                        {
                            string HrefElementPath = node2.Attributes["href"].Value;
                            urlListe.Add(ScraperMethods.MmogaGermany + HrefElementPath);
                        }
                    }
                }
            }
            for (int i = 0; i < urlListe.Count; i++)
            {
                Product prod = ScraperMethods.GetProductType(urlListe[i]);
                if (prod != null)
                {
                    ProductList.Add(prod);
                }
            }
            return ProductList;
        }

        private static void ThrowTooShortError()
        {
            Exception ShortQuery = new("The given query string has to be at least three characters long to search!");
            throw ShortQuery;
        }

        public static List<LightProduct> QuickSearch(string query)
        {
            if (query.Length < 3)
            {
                ThrowTooShortError();
            }
            List<LightProduct> LightProductList = new();

            using (HttpClient client = new())
            {
                string result = QueryLinkGetResult(query, client);
                if (!object.Equals(result, null))
                {
                    HtmlDocument doc = new();
                    doc.LoadHtml(result);
                    int a = 0;
                    foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='searchCont']"))
                    {
                        a++;
                        LightProductList.Add(ScraperMethods.ParseProductListItem(a, node));
                    }
                }
            }
            return LightProductList;
        }

        private static string QueryLinkGetResult(string query, HttpClient client)
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd(ScraperMethods.Useragent);
            string result = String.Empty;
            using (HttpResponseMessage response = client.GetAsync(ScraperMethods.MmogaSearchBase + query).Result)
            {
                using (HttpContent content = response.Content)
                {
                    result = content.ReadAsStringAsync().Result;
                }
            }

            return result;
        }
    }
}