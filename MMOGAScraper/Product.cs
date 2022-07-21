namespace MMOGAScraper
{
    public class Product
    {
        public Product(string productType, string link, string title, string price, bool isPriceReduced, string reducedPrice, string platformLogo, string deliveryTime, string availability, string region, string platform, string description, bool isPaypalAvailable)
        {
            ProductType = productType;
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
            IsPaypalAvailable = isPaypalAvailable;
        }

        public string ProductType { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Price { get; set; }
        public bool IsPriceReduced { get; set; }
        public string ReducedPrice { get; set; }
        public string PlatformLogo { get; set; }
        public string DeliveryTime { get; set; }
        public string Availability { get; set; }
        public string Region { get; set; }
        public string Platform { get; set; }
        public string Description { get; set; }
        public bool IsPaypalAvailable { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(ProductType)}={ProductType}, {nameof(Link)}={Link}, {nameof(Title)}={Title}, {nameof(Price)}={Price}, {nameof(IsPriceReduced)}={IsPriceReduced.ToString()}, {nameof(ReducedPrice)}={ReducedPrice}, {nameof(PlatformLogo)}={PlatformLogo}, {nameof(DeliveryTime)}={DeliveryTime}, {nameof(Availability)}={Availability}, {nameof(Region)}={Region}, {nameof(Platform)}={Platform}, {nameof(Description)}={Description}, {nameof(IsPaypalAvailable)}={IsPaypalAvailable.ToString()}}}";
        }
    }
}