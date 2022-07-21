# MMOGA Product Scraper - MMOGA-PS (API)

#### Projekt von https://github.com/liebki

A scraper to get all products including the (possible) data of https://mmoga.de

## Technologies

### Created using
- .NET Core 6.0

### Nuget(s)
- HtmlAgilityPack

## Features

### What data is available right now?
- Following data is available for the product objects: 
	- Is the product not available?, Category (steam game, ea game etc.), Cover image, Type of product, Product link, Title, Price, Is the price reduced, Reduced price, Platform logo image, Delivery time, Availability, Region, Platform, Description, Extended description, Is paypal available

### General
- Get a list of product objects, those can be used to get prices, availabilities etc.
- Some products like accounts that are sold on mmoga or fifa points etc. are on my TO-DO but are not high priority
- Example for the type of product that this tool wont support for a bit: [Minecraft Unique Account](https://www.mmoga.de/Minecraft/Minecraft-Unique-Account,Minecraft-Java-Edition-MOJANG-LOG-IN-MAIL-ACCESS--NO-BAN-Hypixel--Fast-delivery/)

## Usage

### Code/Methods to use

The MmogaScraper contains the GetListOfProducts method, this is the only available method right now to get the products, more is on it's way!

```
List<Product> FoundProducts = MmogaScraper.GetListOfProducts("What you desire");
```

## Example

### Code (Is in Program.cs too)
```
string query = "minecraft"; //What to search for on mmoga.de?

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
```

### Output
![Logo](https://iili.io/NDfhHF.png)

## FAQ

#### Does this work on every mmoga page?

I created this for mmoga.DE I don't know about mmoga.com or other regions in general.

#### Why can't I obtain data X?

I'm trying my best, so please be patient or include the things you like to see yourself or contact me :)

#### Why isn't this tool working?

Right now in July of 2022, this tool is working pretty good

## License

**Software:** MMOGA Product Scraper (MMOGA-PS)

**License:** GNU General Public License v3.0

**Licensor:** Kim Mario Liebl

[GNU](https://choosealicense.com/licenses/gpl-3.0/)

## Roadmap

#### Important
- Make this thing a real API with many methods etc.
- Include more pages than just the first page of the search
- Support the most types of products

#### Second choice
- Clean up the code and split it in methods etc.
- Learn more about parsing HTML etc. using HtmlAgilityPack
