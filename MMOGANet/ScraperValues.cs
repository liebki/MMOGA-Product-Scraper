namespace MMOGAScraper
{
    public enum ProductType
    {
        Error = 0,
        Game = 1,
        Dlc = 2,
        Giftcard = 3,
        RandomObject = 4,
        ConsoleGame = 5,
    }

    public enum ProductAvailability
    {
        Error = 0,
        Deliverable = 1,
        Undeliverable = 2,
        Preorder = 3,
    }

    public enum ProductDeliveryTime
    {
        Error = 0,
        FiveToTenMinutes = 1,
        ThreeToFourDays = 2,
        CustomDate = 3,
        Undeliverable = 4,
        Unknown = 5,
    }

    public enum ScraperRegion
    {
        DE = 0,
        US = 1,
        UK = 2,
    }
}