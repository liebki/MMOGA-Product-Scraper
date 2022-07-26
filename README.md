# MMOGA Product Scraper - MMOGA-PS (API)

#### Projekt von https://github.com/liebki

A scraper to get products including the (possible) data of https://mmoga.de

## Technologies

### Created using
- .NET Core 6.0

### Nuget(s)
- HtmlAgilityPack

## Features

### What data is available right now?
- To see what data is available, please take a look in the Product.cs and/or LightProduct.cs 

### General
- Get a list of product objects, those can be used to get prices, availabilities, platforms etc.
- Some products wont be found using the DeeperSearch, because atm I didn't include a way to parse them
- To get **every** (possible made) product, use the QuickSearch but you will loose detail
- I will include every product in the DeeperSearch but it is one of my things on the TO-DO so you have to wait

## Usage

### Code/Methods to use

The MmogaScraper class contains a "QuickSearch" and a "DeeperSearch" method.
QuickSearch will provide you faster results but with fewer detail, it will miss following data of the DeeperSearch:
	Type
	Platform/Logo
	Delivery time
	Region
	Description
	Extended description
	Paypal availability
	
Because of this there is a LightProduct (QuickSearch) and a Product (DeeperSearch).

```
//Get the number of pages a query produces
int QueryPagenumber = MmogaScraper.PagenumberSearch("what I got");

//Get a list of products, faster but with fewer data/details
List<LightProduct> LighProductList = MmogaScraper.QuickSearch("what you want");

//Get all the data, using the slower search
List<Product> ProductList = MmogaScraper.DeeperSearch("what you need");

//This version of DeeperSearch is not 100% finished, tho it is working!
//Get all the data, using the slower search AND search on more than just the first page
List<Product> ProductListAlpha = MmogaScraper.DeeperSearch_Alpha("search them good", pages_as_int);
```

## Example

### Code (Program.cs)
Please take a look in the *Program.cs*, the code there is showing a working example for scraping "fifa", using Quick- and DeeperSearch.

### Output
GIF shows content of "cyberpunk" scraping:
![Logo](https://iili.io/ksX3Vp.gif)

## FAQ

#### Does this work on every mmoga page?

I created this for mmoga.**DE** I don't know about mmoga.com or other regions in general

#### Why can't I obtain data X?

I'm trying my best, so please be patient or include the things you like to see yourself or contact me :)

#### Is it working?

Right now in end of July '22, this tool is working pretty good

## License

**Software:** MMOGA Product Scraper (MMOGA-PS)

**License:** GNU General Public License v3.0

**Licensor:** Kim Mario Liebl

[GNU](https://choosealicense.com/licenses/gpl-3.0/)

## Roadmap

#### Sorted by importancy
- Include more pages than just the first page of the search (WIP, 50% done)
- Option to change region of mmoga
- Parse "coming soon" products
- Parse "hits" products
- Parse "preorder" products
- Parse the payment methods
- Support more types of products
- Learn more about parsing websites etc. using HtmlAgilityPack

## DISCLAIMER SECTION

#### [Read the full disclaimer in the DISCLAIMER.md file!](https://github.com/liebki/MMOGA-Product-Scraper/blob/master/DISCLAIMER.md)

**liebki (me) or this project** isn’t endorsed by MMOGA/WEIT and doesn’t reflect the 
views or opinions of MMOGA/WEIT or anyone officially involved in managing it.
MMOGA is a trademark and/or registered trademark of "WEIT".