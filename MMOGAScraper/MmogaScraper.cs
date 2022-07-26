using HtmlAgilityPack;

namespace MMOGAScraper
{
    public class MmogaScraper
    {
        private ScraperRegion Region;

        public MmogaScraper(ScraperRegion RegionSelection = ScraperRegion.DE)
        {
            if (RegionSelection != ScraperRegion.DE)
            {
                // This feature will be implemented, right now it's just the base-code
                RegionSelection = ScraperRegion.DE;
            }
            Region = RegionSelection;
        }

        public List<object> GetSoonReleasedProducts()
        {
            // I am implementing more and more :)
            return new();
        }

        public int PagenumberSearch(string query)
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

        public List<Product> DeeperSearch(string query, int pages = 1)
        {
            if (pages <= 1)
            {
                throw new ArgumentException("The value for maxpages, must be atleast 1");
            }
            ScraperMethods.CheckQueryLength(query);
            List<Product> ProductList = new();
            List<string> urlListe = new();

            using (HttpClient client = new())
            {
                string result = ScraperMethods.QueryLinkGetResult(query, client);
                HtmlDocument doc = new();
                if (!string.IsNullOrEmpty(result))
                {
                    doc.LoadHtml(result);

                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/form/div[1]/div/span")?.InnerText.Contains("keine Ergebnisse") == true)
                    {
                        return ProductList;
                    }
                    int a = 0;

                    int MaxPagesScraped = ScraperMethods.CalculateQueryPageNumber(doc);
                    if (pages > MaxPagesScraped)
                    {
                        throw new ArgumentException($"You cant ask for {pages} page/s when {query} only provides {MaxPagesScraped} page/s");
                    }

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

        public List<Product> DeeperSearch_Alpha(string query, int pages = 1)
        {
            if (pages < 1)
            {
                throw new ArgumentException("The value for maxpages, must be atleast 1");
            }
            ScraperMethods.CheckQueryLength(query);
            List<Product> ProductList = new();
            List<string> urlListe = new();

            using (HttpClient client = new())
            {
                string result = ScraperMethods.QueryLinkGetResult(query, client);
                HtmlDocument doc = new();
                if (!string.IsNullOrEmpty(result))
                {
                    doc.LoadHtml(result);

                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/form/div[1]/div/span")?.InnerText.Contains("keine Ergebnisse") == true)
                    {
                        return ProductList;
                    }
                    int MaxPagesScraped = ScraperMethods.CalculateQueryPageNumber(doc);
                    if (pages > MaxPagesScraped)
                    {
                        throw new ArgumentException($"You cant ask for {pages} page/s when {query} only provides {MaxPagesScraped} page/s");
                    }

                    for (int i = 1; i <= pages; i++)
                    {
                        urlListe.AddRange(DeeperSearch_AlphaParsing(query, i));
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

        private List<string> DeeperSearch_AlphaParsing(string query, int page)
        {
            List<string> urlListe = new();
            using (HttpClient client = new())
            {
                string result = ScraperMethods.QueryLinkGetResult(query, client, page);
                HtmlDocument doc = new();
                if (!string.IsNullOrEmpty(result))
                {
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
            return urlListe;
        }

        public List<LightProduct> QuickSearch(string query, int pages = 1)
        {
            if (pages < 1)
            {
                throw new ArgumentException("The value for maxpages, must be atleast 1");
            }
            ScraperMethods.CheckQueryLength(query);
            List<LightProduct> LightProductList = new();

            using (HttpClient client = new())

            {
                string result = ScraperMethods.QueryLinkGetResult(query, client);
                HtmlDocument doc = new();
                if (!string.IsNullOrEmpty(result))
                {
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