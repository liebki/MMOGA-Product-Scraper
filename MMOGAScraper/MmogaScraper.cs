using HtmlAgilityPack;

namespace MMOGAScraper
{
    public static class MmogaScraper
    {
        public static List<object> GetSoonReleasedProducts()
        {
            // I am implementing more and more :)
            return new();
        }

        public static int PagenumberSearch(string query)
        {
            ScraperMethods.CheckQueryLength(query);

            int pages = 0;
            using (HttpClient client = new())
            {
                string result = ScraperMethods.QueryLinkGetResult(query, client);
                if (!object.Equals(result, null))
                {
                    HtmlDocument doc = new();
                    doc.LoadHtml(result);
                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/form/div[1]/div/span")?.InnerText.Contains("keine Ergebnisse") == true)
                    {
                        return pages;
                    }
                    pages = ScraperMethods.CalculateQueryPageNumber(doc);
                }
            }
            return pages;
        }

        public static List<Product> DeeperSearch(string query, int maxpages = 1)
        {
            ScraperMethods.CheckQueryLength(query);
            List<Product> ProductList = new();
            List<string> urlListe = new();

            using (HttpClient client = new())
            {
                string result = ScraperMethods.QueryLinkGetResult(query, client);
                if (!object.Equals(result, null))
                {
                    HtmlDocument doc = new();
                    doc.LoadHtml(result);
                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/form/div[1]/div/span")?.InnerText.Contains("keine Ergebnisse") == true)
                    {
                        return ProductList;
                    }
                    int a = 0;

                    int MaxPagesForProduct = ScraperMethods.CalculateQueryPageNumber(doc);

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

        public static List<LightProduct> QuickSearch(string query, int maxpages = 1)
        {
            ScraperMethods.CheckQueryLength(query);
            List<LightProduct> LightProductList = new();

            using (HttpClient client = new())
            {
                string result = ScraperMethods.QueryLinkGetResult(query, client);
                if (!object.Equals(result, null))
                {
                    HtmlDocument doc = new();
                    doc.LoadHtml(result);
                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/form/div[1]/div/span")?.InnerText.Contains("keine Ergebnisse") == true)
                    {
                        return LightProductList;
                    }
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
    }
}