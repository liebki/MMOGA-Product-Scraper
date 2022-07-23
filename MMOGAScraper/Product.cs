namespace MMOGAScraper
{
    public class Product
    {
        public Product(bool isNotAvailable, string shopCategory, string cover, ProductType type, string link, string title, decimal price, bool isPriceReduced, decimal reducedPrice, string platformLogo, string deliveryTime, string availability, string region, string platform, string description, string extendedDescription, bool isPaypalAvailable)
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
            Availability = availability;
            Region = region;
            Platform = platform;
            Description = description;
            ExtendedDescription = extendedDescription;
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
        public string DeliveryTime { get; set; }
        public string Availability { get; set; }
        public string Region { get; set; }
        public string Platform { get; set; }
        public string Description { get; set; }
        public string ExtendedDescription { get; set; }
        public bool IsPaypalAvailable { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(IsNotAvailable)}={IsNotAvailable}, {nameof(ShopCategory)}={ShopCategory}, {nameof(Cover)}={Cover}, {nameof(Type)}={Type}, {nameof(Link)}={Link}, {nameof(Title)}={Title}, {nameof(Price)}={Price}, {nameof(IsPriceReduced)}={IsPriceReduced}, {nameof(ReducedPrice)}={ReducedPrice}, {nameof(PlatformLogo)}={PlatformLogo}, {nameof(DeliveryTime)}={DeliveryTime}, {nameof(Availability)}={Availability}, {nameof(Region)}={Region}, {nameof(Platform)}={Platform}, {nameof(Description)}={Description}, {nameof(ExtendedDescription)}={ExtendedDescription}, {nameof(IsPaypalAvailable)}={IsPaypalAvailable}}}";
        }
    }
}