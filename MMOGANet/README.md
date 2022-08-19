# MMOGANet
A scraper to get products (and their data) of https://mmoga.de

Example code:
```
MmogaScraper scraper = new(ScraperRegion.DE);

const string querystring = "fifa"; 
const int pages = 2;
Console.WriteLine($"Querying: {querystring} for {pages} pages");

Console.WriteLine("Page number search:");
int QueryPagenumber = scraper.PagenumberSearch(querystring);
Console.WriteLine($"Number of pages: {QueryPagenumber} for: {querystring}");

Console.WriteLine(Environment.NewLine);

Console.WriteLine("Quick search:");
List<LightProduct> LighProductList = scraper.QuickSearch(querystring);
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
```