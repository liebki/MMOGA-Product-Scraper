using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;

namespace MMOGAScraper
{
    public class MmogaScraper
    {
        /*
         * This tool was built for the german mmoga.de site, no other region of mmoga was tested using this code or is covered by tests!
         */

        #region Contants

        private const string Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:91.0) Gecko/20100101 Firefox/91.0";
        private const string MmogaSearchBase = "https://www.mmoga.de/advanced_search.php?keywords=";
        private const string MmogaGermany = "https://www.mmoga.de";

        #endregion Contants

        private static Product GetProductType(string ProductLink)
        {
            #region Httpclient url query

            string result = String.Empty;
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(Useragent);
                using (HttpResponseMessage response = client.GetAsync(ProductLink).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        result = content.ReadAsStringAsync().Result;
                    }
                }
            }

            #endregion Httpclient url query

            if (!object.Equals(result, null))
            {
                bool IsDlc = false;
                bool IsGiftcard = false;
                bool IsRandomObject = false;
                bool IsConsoleGame = false;

                HtmlDocument doc = new();
                doc.LoadHtml(result);
                if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[2]/h1") != null)
                {
                    #region Product simple description

                    StringBuilder ProductDescriptionBuilder = new();
                    HtmlNodeCollection TextLines = doc.DocumentNode.SelectNodes("//div[contains(@class, 'prodtUpCent')]");
                    foreach (HtmlNode node in TextLines)
                    {
                        ProductDescriptionBuilder.Append(node.InnerHtml);
                    }
                    string ProductDescription = new(StripHtmlTags(ProductDescriptionBuilder.ToString()));
                    ProductDescription = Regex.Replace(ProductDescription, @"(?:\s)\s", "");

                    #endregion Product simple description

                    #region Product title

                    string ProductTitle = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[2]/h1").InnerHtml;

                    #endregion Product title

                    #region Product type selection

                    if (ProductTitle.Contains("-DLC") || ProductLink.Contains("DLC") || ProductDescription.Contains("DLC"))
                    {
                        IsDlc = true;
                    }
                    if (ProductDescription.Contains("Card Code") || ProductDescription.Contains("Guthabenkarte") || ProductDescription.Contains("Gift Card") || ProductDescription.Contains("Prepaid Card"))
                    {
                        IsGiftcard = true;
                    }
                    if (ProductTitle.Contains("random", StringComparison.OrdinalIgnoreCase) || ProductLink.Contains("random", StringComparison.OrdinalIgnoreCase) || ProductDescription.Contains("random", StringComparison.OrdinalIgnoreCase) || ProductDescription.Contains("random key", StringComparison.OrdinalIgnoreCase) || ProductDescription.Contains("random key", StringComparison.OrdinalIgnoreCase))
                    {
                        IsRandomObject = true;
                    }
                    if (ProductTitle.Contains("xbox", StringComparison.OrdinalIgnoreCase) || ProductLink.Contains("xbox", StringComparison.OrdinalIgnoreCase) || ProductDescription.Contains("xbox", StringComparison.OrdinalIgnoreCase) || ProductTitle.Contains("PS4") || ProductTitle.Contains("PS5") || ProductLink.Contains("Playstation-Network", StringComparison.OrdinalIgnoreCase) || ProductDescription.Contains("Playstation", StringComparison.OrdinalIgnoreCase) || ProductTitle.Contains("Nintendo Switch Download Code", StringComparison.OrdinalIgnoreCase) || ProductLink.Contains("Nintendo-Switch-Download-Code", StringComparison.OrdinalIgnoreCase))
                    {
                        IsConsoleGame = true;
                    }

                    ProductType Type;
                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[5]/ul/li[2]/p") != null && !IsDlc && !IsGiftcard && !IsRandomObject)
                    {
                        Type = ProductType.Game;
                    }
                    else if (IsDlc)
                    {
                        Type = ProductType.Dlc;
                    }
                    else if (IsGiftcard)
                    {
                        Type = ProductType.Giftcard;
                    }
                    else if (IsRandomObject)
                    {
                        Type = ProductType.RandomObject;
                    }
                    else if (IsConsoleGame)
                    {
                        Type = ProductType.ConsoleGame;
                    }
                    else
                    {
                        Type = ProductType.Unknown;
                    }

                    #endregion Product type selection

                    #region Price and/or availability

                    string Price = String.Empty;
                    bool PriceReduced = false;
                    string ReducedPrice = String.Empty;
                    bool IsGameNotAvailable = false;

                    HtmlNode PriceBlock = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[3]/div");
                    string PriceBlockValue = StripHtmlTags(PriceBlock.InnerHtml);
                    if (PriceBlockValue.Contains("Benachrichtigen", StringComparison.OrdinalIgnoreCase))
                    {
                        IsGameNotAvailable = true;
                    }
                    else
                    {
                        Price = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[3]/div[2]/p").InnerHtml;
                        if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[3]/div[2]/del") != null)
                        {
                            PriceReduced = true;
                            ReducedPrice = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[3]/div[2]/del").InnerHtml;
                        }
                        Price = Price.Replace("&nbsp;&euro;", "");
                        ReducedPrice = ReducedPrice.Replace("&nbsp;&euro;", "");
                        Price = Price.Replace("UVP ", "");
                        ReducedPrice = ReducedPrice.Replace("UVP ", "");
                    }

                    #endregion Price and/or availability

                    #region Platform image logo source

                    string PlatformLogoSource = String.Empty;
                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[1]/img") != null)
                    {
                        HtmlNode PlatformLogo = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[1]/img");
                        PlatformLogoSource = MmogaGermany + PlatformLogo.SelectSingleNode("//img").Attributes["src"].Value;
                    }

                    #endregion Platform image logo source

                    #region Product cover image

                    string CoverImage = String.Empty;
                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[1]/div[2]/div[1]/img") != null)
                    {
                        HtmlNode PlatformLogo = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[1]/div[2]/div[1]/img");
                        CoverImage = MmogaGermany + PlatformLogo.Attributes["src"].Value;
                    }

                    #endregion Product cover image

                    #region Product category

                    string ShopCategory = String.Empty;
                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[1]/a[2]") != null)
                    {
                        ShopCategory = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[1]/a[2]").InnerHtml;
                    }

                    #endregion Product category

                    #region Detailed product description

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
                    string DescriptionText = StripHtmlTags(Description.InnerHtml);
                    DescriptionText = Regex.Replace(DescriptionText, @"(?:\s)\\s", "");
                    DescriptionText = Regex.Replace(DescriptionText, @"\t|\n|\r", "");

                    #endregion Detailed product description

                    #region Paypal available

                    bool IsPaypalAvailable = false;
                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[3]/div[1]/div") != null)
                    {
                        IsPaypalAvailable = true;
                    }

                    #endregion Paypal available

                    #region Product object

                    Product product = new(IsGameNotAvailable, ShopCategory, CoverImage, Type, ProductLink, ProductTitle, Price, PriceReduced, ReducedPrice, PlatformLogoSource, DeliveryTime, Availability, Region, Platform, ProductDescription, DescriptionText, IsPaypalAvailable);

                    #endregion Product object

                    return product;
                }
            }
            return null;
        }

        private static string StripHtmlTags(string html)
        {
            if (String.IsNullOrEmpty(html)) return "";
            HtmlDocument doc = new();
            doc.LoadHtml(html);
            return doc.DocumentNode.InnerText;
        }

        public static List<Product> GetListOfProducts(string gamename)
        {
            List<Product> ProductList = new();
            List<string> urlListe = new();

            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd(Useragent);
                string result = String.Empty;
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
                            string HrefElementPath = node2.Attributes["href"].Value;
                            urlListe.Add(MmogaGermany + HrefElementPath);
                        }
                    }
                }
            }
            for (int i = 0; i < urlListe.Count; i++)
            {
                Product prod = GetProductType(urlListe[i]);
                if (prod != null)
                {
                    ProductList.Add(prod);
                }
            }
            return ProductList;
        }
    }
}