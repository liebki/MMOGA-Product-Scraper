namespace MMOGAScraper
{
    internal class Program
    {
        public static void Main()
        {
            const string querystring = "fifa"; //What to search for on mmoga.de
            Console.WriteLine($"Searching for {querystring} keyword");

            #region Quick search example

            Console.WriteLine("Quick search:");
            List<LightProduct> LighProductList = MmogaScraper.QuickSearch(querystring);
            if (LighProductList.Count > 0)
            {
                foreach (LightProduct product in LighProductList)
                {
                    PrintLightProduct(product);
                }
            }
            else
            {
                Console.WriteLine($"No data found for query {querystring}");
            }

            #endregion Quick search example

            Console.WriteLine(Environment.NewLine);

            #region Deeper search example

            Console.WriteLine("Deeper search:");
            List<Product> ProductList = MmogaScraper.DeeperSearch(querystring);
            if (ProductList.Count > 0)
            {
                foreach (Product product in ProductList)
                {
                    PrintProduct(product);
                }
            }
            else
            {
                Console.WriteLine($"No data found for query {querystring}");
            }

            #endregion Deeper search example
        }

        #region Just print stuff

        private static void PrintProduct(Product product)
        {
            Console.WriteLine("----------------------------------");
            string ProductData = $" \nCategory: {product.ShopCategory} \nAvailability: {product.Availability}";
            if (product.IsNotAvailable)
            {
                ProductData += "\nPrice: /";
            }
            else
            {
                ProductData += $"\nPrice: {product.Price}\nReduced price: {product.ReducedPrice}";
            }
            if (product.Type == ProductType.Game)
            {
                ProductData = $"Game: {product.Title}" + ProductData;
            }
            else
            {
                ProductData = $"Product: {product.Title}" + ProductData;
            }
            Console.WriteLine(ProductData);
            if (product.DeliveryTime == ProductDeliveryTime.CustomDate)
            {
                Console.WriteLine($"Delivery time: {product.DeliveryTimeCustomDate}");
            }
            else
            {
                Console.WriteLine($"Delivery time: {product.DeliveryTime}");
            }
            Console.WriteLine($"Paypal available: {product.IsPaypalAvailable}");
            Console.WriteLine($"Region: {product.Region}");
            Console.WriteLine($"Platform: {product.Platform}");
            Console.WriteLine("----------------------------------");
            Console.WriteLine(Environment.NewLine);
        }

        private static void PrintLightProduct(LightProduct lightproduct)
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"Titel: {lightproduct.Title}");
            Console.WriteLine($"Link: {lightproduct.Link}");
            Console.WriteLine($"Kategorie: {lightproduct.ShopCategory}");
            if (lightproduct.IsNotAvailable)
            {
                Console.WriteLine("Spiel ist nicht verfügbar!");
                Console.WriteLine("Preis: /");
            }
            else
            {
                if (lightproduct.IsPriceReduced)
                {
                    Console.WriteLine($"Reduzierter Preis: {lightproduct.ReducedPrice}");
                }
                Console.WriteLine($"Preis: {lightproduct.Price}");
            }
            Console.WriteLine($"Coverimage: {lightproduct.CoverImage}");
            Console.WriteLine("----------------------------------");
            Console.WriteLine(Environment.NewLine);
        }

        #endregion Just print stuff
    }
}