namespace MMOGAScraper
{
    public abstract class Product
    {
        public string Systemrequirements { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Deliverytime { get; set; }
        public bool Available { get; set; }
        public string Region { get; set; }
        public string Platform { get; set; }
        public string Price { get; set; }
        public bool Reduced { get; set; }
        public string Reducedprice { get; set; }
        public string Productinfo { get; set; }
        public bool Paypalavailable { get; set; }
        public string Productmaincategory { get; set; }
        public bool Ispcgame { get; set; }
    }
}