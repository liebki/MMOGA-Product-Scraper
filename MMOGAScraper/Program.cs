using HtmlAgilityPack;
using System.Text;

namespace MMOGAScraper
{
    internal class Program
    {
        /*
         * This program is built for the german mmoga.de site, no other region of mmoga was tested using this code!
         */

        private const string Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:91.0) Gecko/20100101 Firefox/91.0";
        private const string MmogaSearchBase = "https://www.mmoga.de/advanced_search.php?keywords=";
        private const string MmogaGermany = "https://www.mmoga.de";
        private const string Xpath_ProductTitle = "/html/body/div[2]/div/div[2]/h1";

        public static void Main()
        {
            GetPriceOfGame("cyberpunk");
        }

        private static Product GetProductType(string v)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.UserAgent.ParseAdd(Useragent);
            string result = "";
            using (HttpResponseMessage response = client.GetAsync(v).Result)
            {
                using (HttpContent content = response.Content)
                {
                    result = content.ReadAsStringAsync().Result;
                }
            }
            if (!object.Equals(result, null))
            {
                bool IsDlc = false;
                bool IsGiftcard = false;
                bool IsRandomObject = false;
                bool IsConsoleGame = false;

                HtmlDocument doc = new();
                doc.LoadHtml(result);
                if (doc.DocumentNode.SelectSingleNode(Xpath_ProductTitle) != null)
                {
                    string ProductTitle = doc.DocumentNode.SelectSingleNode(Xpath_ProductTitle).InnerHtml;
                    StringBuilder ProductDescription = new();

                    HtmlNodeCollection TextLines = doc.DocumentNode.SelectNodes("//div[contains(@class, 'prodtUpCent')]");
                    foreach (HtmlNode node in TextLines)
                    {
                        ProductDescription.Append(node.InnerHtml);
                    }
                    ProductDescription = new(GetContentInTags(ProductDescription.ToString()));

                    if (ProductTitle.Contains("-DLC") || v.Contains("DLC") || ProductDescription.ToString().Contains("DLC"))
                    {
                        IsDlc = true;
                    }
                    if (ProductDescription.ToString().Contains("Card Code") || ProductDescription.ToString().Contains("Guthabenkarte") || ProductDescription.ToString().Contains("Gift Card") || ProductDescription.ToString().Contains("Prepaid Card"))
                    {
                        IsGiftcard = true;
                    }
                    if (ProductTitle.Contains("random", StringComparison.OrdinalIgnoreCase) || v.Contains("random", StringComparison.OrdinalIgnoreCase) || ProductDescription.ToString().Contains("random", StringComparison.OrdinalIgnoreCase) || ProductDescription.ToString().Contains("random key", StringComparison.OrdinalIgnoreCase) || ProductDescription.ToString().Contains("random key", StringComparison.OrdinalIgnoreCase))
                    {
                        IsRandomObject = true;
                    }
                    if (ProductTitle.Contains("xbox", StringComparison.OrdinalIgnoreCase) || v.Contains("xbox", StringComparison.OrdinalIgnoreCase) || ProductDescription.ToString().Contains("xbox", StringComparison.OrdinalIgnoreCase) || ProductTitle.Contains("PS4") || ProductTitle.Contains("PS5") || v.Contains("Playstation-Network", StringComparison.OrdinalIgnoreCase) || ProductDescription.ToString().Contains("Playstation", StringComparison.OrdinalIgnoreCase) || ProductTitle.Contains("Nintendo Switch Download Code", StringComparison.OrdinalIgnoreCase) || v.Contains("Nintendo-Switch-Download-Code", StringComparison.OrdinalIgnoreCase))
                    {
                        IsConsoleGame = true;
                    }

                    string ProductType;
                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[5]/ul/li[2]/p") != null && !IsDlc && !IsGiftcard && !IsRandomObject)
                    {
                        ProductType = "Game";
                    }
                    else if (IsDlc)
                    {
                        ProductType = "DLC";
                    }
                    else if (IsGiftcard)
                    {
                        ProductType = "GiftCard";
                    }
                    else if (IsRandomObject)
                    {
                        ProductType = "Random Key or Item";
                    }
                    else if (IsConsoleGame)
                    {
                        ProductType = "Console";
                    }
                    else
                    {
                        ProductType = "No Game, DLC, GiftCard or Random Object!";
                    }

                    HtmlNode Price = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[3]/div[2]/p");
                    bool PriceReduced = false;
                    string ReducedPrice = String.Empty;
                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[3]/div[2]/del") != null)
                    {
                        PriceReduced = true;
                        ReducedPrice = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[3]/div[2]/del").InnerHtml;
                    }

                    string PlatformLogoSource = String.Empty;
                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[1]/img") != null)
                    {
                        HtmlNode PlatformLogo = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[1]/img");
                        PlatformLogoSource = MmogaGermany + PlatformLogo.SelectSingleNode("//img").Attributes["src"].Value;
                    }

                    HtmlNode InfoDetailTableBlock = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[2]/div");

                    List<string> InfoDetails = new();
                    foreach (HtmlNode row in InfoDetailTableBlock.SelectNodes("//tr"))
                    {
                        foreach (HtmlNode cell in row.SelectNodes("//td"))
                        {
                            InfoDetails.Add(cell.InnerText);
                        }
                    }

                    string DeliveryTime = InfoDetails[0];
                    string Availability = InfoDetails[1];
                    string Region = InfoDetails[2];
                    string Platform = InfoDetails[3];

                    HtmlNode Description = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[5]/ul/li[1]/div");
                    string DescriptionText = GetContentInTags(Description.InnerHtml);
                    DescriptionText = DescriptionText.Replace("(?:\\s)\\s", " ");

                    bool IsPaypalAvailable = false;
                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[3]/div[1]/div") != null)
                    {
                        IsPaypalAvailable = true;
                    }

                    Product product = new(ProductType, v, ProductTitle, Price.InnerHtml, PriceReduced, ReducedPrice, PlatformLogoSource, DeliveryTime, Availability, Region, Platform, DescriptionText, IsPaypalAvailable);
                    return product;
                }
                else
                {
                    Console.WriteLine("Skip more custom pages");
                }
            }
            return null;
        }

        private static string GetContentInTags(string html)
        {
            HtmlDocument mainDoc = new();
            mainDoc.LoadHtml(html);
            string cleanText = mainDoc.DocumentNode.InnerText;
            return cleanText;
        }

        private static void GetPriceOfGame(string gamename)
        {
            List<string> urlListe = new();
            HttpClient client = new();
            client.DefaultRequestHeaders.UserAgent.ParseAdd(Useragent);
            string result = "";
            using (HttpResponseMessage response = client.GetAsync(MmogaSearchBase + gamename).Result)
            {
                using (HttpContent content = response.Content)
                {
                    result = content.ReadAsStringAsync().Result;
                }
            }
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
                        string itemCont = node2.Attributes["href"].Value;
                        urlListe.Add(MmogaGermany + itemCont);
                        Console.WriteLine($"Link: {MmogaGermany}{itemCont}");
                    }
                }
            }
            List<Product> ProductList = new();
            Console.WriteLine($"{urlListe.Count} entries found\n");
            for (int i = 0; i < urlListe.Count; i++)
            {
                Product prod = GetProductType(urlListe[i]);
                if (prod != null)
                {
                    ProductList.Add(prod);
                }
            }
            Console.WriteLine($"{ProductList.Count} entries in product list!\n");
        }
    }
}