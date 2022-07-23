namespace MMOGAScraper
{
    public class LightProduct
    {
        public LightProduct(bool isNotAvailable, string smallCover, string shopCategory, string link, string title, decimal price, bool isPriceReduced, decimal reducedPrice)
        {
            IsNotAvailable = isNotAvailable;
            CoverImage = smallCover;
            ShopCategory = shopCategory;
            Link = link;
            Title = title;
            Price = price;
            IsPriceReduced = isPriceReduced;
            ReducedPrice = reducedPrice;
        }

        public bool IsNotAvailable { get; set; }
        public string CoverImage { get; set; }
        public string ShopCategory { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public bool IsPriceReduced { get; set; }
        public decimal ReducedPrice { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(IsNotAvailable)}={IsNotAvailable}, {nameof(CoverImage)}={CoverImage}, {nameof(ShopCategory)}={ShopCategory}, {nameof(Link)}={Link}, {nameof(Title)}={Title}, {nameof(Price)}={Price}, {nameof(IsPriceReduced)}={IsPriceReduced}, {nameof(ReducedPrice)}={ReducedPrice}}}";
        }
    }
}