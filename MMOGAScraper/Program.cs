namespace MMOGAScraper
{
    internal class Program
    {
        public static void Main()
        {
            string query = "cyberpunk"; //What to search for on mmoga.de

            List<Product> FoundProducts = MmogaScraper.GetListOfProducts(query);
            Console.WriteLine($"I found {FoundProducts.Count} good products for {query}");
            foreach (Product product in FoundProducts)
            {
                Console.WriteLine("----------------------------------");
                string ProductData = $" \nCategory: {product.ShopCategory} \nAvailability: {product.Availability}";
                if (product.IsNotAvailable)
                {
                    ProductData += "\nPrice: /";
                }
                else
                {
                    ProductData += $"\nPrice/Reduced price: {product.Price}/{product.ReducedPrice}";
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
                Console.WriteLine("----------------------------------");
                Console.WriteLine(Environment.NewLine);
            }
        }
    }
}