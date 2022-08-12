namespace MMOGAScraper
{
    public class Product
    {
        public Product(bool isNotAvailable, string shopCategory, string cover, ProductType type, string link, string title, decimal price, bool isPriceReduced, decimal reducedPrice, string platformLogo, ProductDeliveryTime deliveryTime, DateTime deliveryTimeCustomDate, ProductAvailability availability, string region, string platform, string description, string extendedDescription, Requirements systemRequirements, bool isPaypalAvailable)
        {
            IsNotAvailable = isNotAvailable;
            ShopCategory = shopCategory;
            Cover = cover;
            Type = type;
            Link = link;
            Title = title;
            Price = price;
            IsPriceReduced = isPriceReduced;
            ReducedPrice = reducedPrice;
            PlatformLogo = platformLogo;
            DeliveryTime = deliveryTime;
            DeliveryTimeCustomDate = deliveryTimeCustomDate;
            Availability = availability;
            Region = region;
            Platform = platform;
            Description = description;
            ExtendedDescription = extendedDescription;
            SystemRequirements = systemRequirements;
            IsPaypalAvailable = isPaypalAvailable;
        }

        public bool IsNotAvailable { get; set; }
        public string ShopCategory { get; set; }
        public string Cover { get; set; }
        public ProductType Type { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public bool IsPriceReduced { get; set; }
        public decimal ReducedPrice { get; set; }
        public string PlatformLogo { get; set; }
        public ProductDeliveryTime DeliveryTime { get; set; }
        public DateTime DeliveryTimeCustomDate { get; set; }
        public ProductAvailability Availability { get; set; }
        public string Region { get; set; }
        public string Platform { get; set; }
        public string Description { get; set; }
        public string ExtendedDescription { get; set; }
        public Requirements SystemRequirements { get; set; }
        public bool IsPaypalAvailable { get; set; }
    }
}