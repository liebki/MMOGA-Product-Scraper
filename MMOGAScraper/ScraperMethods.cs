using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;

namespace MMOGAScraper
{
    internal static class ScraperMethods
    {
        #region Contants

        internal const string Useragent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:91.0) Gecko/20100101 Firefox/91.0";
        internal const string MmogaSearchBase = "https://www.mmoga.de/advanced_search.php?keywords=";
        internal const string MmogaGermany = "https://www.mmoga.de";

        #endregion Contants

        internal static Product GetProductType(string ProductLink)
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
                        Type = ProductType.Error;
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
                        ReducedPrice = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[3]/div[2]/p").InnerHtml;
                        if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[3]/div[2]/del") != null)
                        {
                            PriceReduced = true;
                            Price = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[3]/div[2]/del").InnerHtml;
                        }
                        else
                        {
                            Price = ReducedPrice;
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

                    HtmlNode Description = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[5]/ul/li[1]/div");
                    string DescriptionText = StripHtmlTags(Description.InnerHtml);
                    DescriptionText = WhitespaceNewLineRemover(DescriptionText);

                    DateTime CustomDeliveryTime;
                    ProductDeliveryTime DeliveryTime;
                    Tuple<ProductDeliveryTime, DateTime> DeliveryData = GetDeliveryTime(InfoDetails[0]);
                    if (DeliveryData.Item1 == ProductDeliveryTime.CustomDate)
                    {
                        DeliveryTime = DeliveryData.Item1;
                        CustomDeliveryTime = DeliveryData.Item2;
                    }
                    else
                    {
                        DeliveryTime = DeliveryData.Item1;
                        CustomDeliveryTime = new();
                    }

                    ProductAvailability Availability = GetAvailability(InfoDetails[1]);
                    string Region = InfoDetails[2];
                    string Platform = InfoDetails[3];

                    #endregion Detailed product description

                    #region Paypal available

                    bool IsPaypalAvailable = false;
                    if (doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[3]/div[2]/div[3]/div[1]/div") != null)
                    {
                        IsPaypalAvailable = true;
                    }

                    #endregion Paypal available

                    #region Product object

                    Product product = new(IsGameNotAvailable, ShopCategory, CoverImage, Type, ProductLink, ProductTitle, ReturnDecimalValue(Price), PriceReduced, ReturnDecimalValue(ReducedPrice), PlatformLogoSource, DeliveryTime, CustomDeliveryTime, Availability, Region, Platform, ProductDescription, DescriptionText, IsPaypalAvailable);

                    #endregion Product object

                    return product;
                }
            }
            return null;
        }

        private static ProductAvailability GetAvailability(string input)
        {
            if (String.Equals(input, "nicht lieferbar", StringComparison.OrdinalIgnoreCase))
            {
                return ProductAvailability.Undeliverable;
            }
            else if (String.Equals(input, "lieferbar", StringComparison.OrdinalIgnoreCase))
            {
                return ProductAvailability.Deliverable;
            }
            else if (String.Equals(input, "Vorbestellbar", StringComparison.OrdinalIgnoreCase))
            {
                return ProductAvailability.Preorder;
            }
            else
            {
                return ProductAvailability.Error;
            }
        }

        private static Tuple<ProductDeliveryTime, DateTime> GetDeliveryTime(string input)
        {
            if (String.Equals(input, "5-10 Minuten", StringComparison.OrdinalIgnoreCase))
            {
                return new(ProductDeliveryTime.FiveToTenMinutes, new());
            }
            else if (String.Equals(input, "3-4 Tage", StringComparison.OrdinalIgnoreCase))
            {
                return new(ProductDeliveryTime.ThreeToFourDays, new());
            }
            else if (String.Equals(input, "Derzeit nicht lieferbar", StringComparison.OrdinalIgnoreCase))
            {
                return new(ProductDeliveryTime.Undeliverable, new());
            }
            else if (String.Equals(input, "Lieferzeit unbekannt", StringComparison.OrdinalIgnoreCase))
            {
                return new(ProductDeliveryTime.Unknown, new());
            }
            else
            {
                if (DateTime.TryParse(input, out DateTime customDate))
                {
                    return new(ProductDeliveryTime.CustomDate, customDate);
                }
                else
                {
                    return new(ProductDeliveryTime.Unknown, new());
                }
            }
        }

        private static string WhitespaceNewLineRemover(string input)
        {
            input = Regex.Replace(input, @"(?:\s)\\s", "");
            input = Regex.Replace(input, @"\t|\n|\r", "");
            return input;
        }

        private static string StripHtmlTags(string html)
        {
            if (String.IsNullOrEmpty(html)) return "";
            HtmlDocument doc = new();
            doc.LoadHtml(html);
            return doc.DocumentNode.InnerText;
        }

        internal static decimal ReturnDecimalValue(string input)
        {
            if (Decimal.TryParse(input, out decimal result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        internal static void CheckQueryLength(string query)
        {
            if (query.Length < 3)
            {
                Exception ShortQuery = new("The given query string has to be at least three characters long to search!");
                throw ShortQuery;
            }
        }

        internal static string QueryLinkGetResult(string query, HttpClient client)
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

        internal static LightProduct ParseProductListItem(int ProductCount, HtmlNode ProductNode)
        {
            HtmlNode Title = ProductNode.SelectSingleNode($"/html/body/div[2]/div/div[2]/div[2]/div[{ProductCount}]/ul/li[2]/a");
            string Link = ScraperMethods.MmogaGermany + Title.Attributes["href"].Value;
            string CoverImage = String.Empty;
            bool IsNotAvailable = true;
            bool IsPriceReduced = false;

            Regex pattern = new("[a-zA-Z]+-[a-zA-Z]+");
            Match match = pattern.Match(Link);
            string Category = match.Groups[0].Value;
            Category = Category.Replace('-', ' ');

            string Price = String.Empty;
            string ReducedPrice = String.Empty;

            if (ProductNode.SelectSingleNode($"/html/body/div[2]/div/div[2]/div[2]/div[{ProductCount}]/ul/li[1]/a") != null)
            {
                CoverImage = ScraperMethods.MmogaGermany + ProductNode.SelectSingleNode($"/html/body/div[2]/div/div[2]/div[2]/div[{ProductCount}]/ul/li[1]/a").Attributes["data-background"].Value;
            }

            if (ProductNode.SelectSingleNode($"/html/body/div[2]/div/div[2]/div[2]/div[{ProductCount}]/ul/li[3]")?.InnerText.Contains("&nbsp;&euro;") == true)
            {
                HtmlNode PriceReducedPrice = ProductNode.SelectSingleNode($"/html/body/div[2]/div/div[2]/div[2]/div[{ProductCount}]/ul/li[3]");

                if (Title.InnerText.Contains("Nickname Changeable | 4+ Letter Nickname | PC, Mac"))
                {
                    Console.WriteLine(Title.InnerText);
                }

                string PriceFiltering = WhitespaceNewLineRemover(PriceReducedPrice.InnerText);
                string[] split = PriceFiltering.Split(new string[] { "&nbsp;&euro;" }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length >= 2)
                {
                    Price = split[0];
                    ReducedPrice = split[1];
                    IsPriceReduced = true;
                }
                else
                {
                    PriceFiltering = PriceFiltering.Replace("&nbsp;&euro;", "");
                    Price = PriceFiltering;
                }
                IsNotAvailable = false;
            }

            return new(IsNotAvailable, CoverImage, Category, Link, Title.InnerText, ReturnDecimalValue(Price), IsPriceReduced, ReturnDecimalValue(ReducedPrice));
        }
    }
}