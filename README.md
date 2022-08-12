# MMOGANet (Scraper for mmoga)

##### Projekt von https://github.com/liebki

## Introduction

### Why does this exist?

I originally needed a way to parse prices for games, for [BlazorLibrary](https://github.com/liebki/BlazorLibrary), now I created this and I might implement it in some time.

### General

So this tool/api, is scraper to get products (and their data) of https://mmoga.de (And soon all other regions like US, UK etc.)

## Technologies

### Created using
- .NET Core 6.0

### Nugets/Dependencies
- [HtmlAgilityPack](https://www.nuget.org/packages/HtmlAgilityPack/)

## Features

### What data is available right now?

To see what data is available, please take a look in the [LightProduct](https://github.com/liebki/MMOGA-Product-Scraper/blob/master/MMOGAScraper/LightProduct.cs)/[Product](https://github.com/liebki/MMOGA-Product-Scraper/blob/master/MMOGAScraper/Product.cs).cs classes 

### General

Get a ```List<LightProduct>``` or ```List<Product>```, the objects inside of the lists (depending on the object), contain data like availabilities, prices and much more.

#### Information

Some products wont be parsed by the "DeeperSearch", to get **every** (parseable) product, use the "QuickSearch" (you will loose some data, compare the LightProduct and Product to know which).
Im working on including every product for the "DeeperSearch" but it is one of my things on the TO-DO so you have to wait

## Usage

### Code/Methods to use

The MmogaScraper class contains a "QuickSearch", "DeeperSearch", "PagenumberSearch" and "DeeperSearch_Alpha" method.
- QuickSearch: Provides you with faster results but with fewer detail
- DeeperSearch: Provides you with the complete data but it is slower
- PagenumberSearch: Provides you with the number of pages a query gives back
- DeeperSearch_Alpha: Provides you with the complete data but it is slower and you can select the number of pages you want to have the data of

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

### Code

Please take a look in the *[Program.cs](https://github.com/liebki/MMOGA-Product-Scraper/blob/master/MMOGAScraper/Program.cs)*, the code there is showing a working example for scraping "fifa", using 
"QuickSearch", "DeeperSearch_Alpha" and "PagenumberSearch".

### Output

GIF shows content of "cyberpunk" scraping:
![Logo](https://iili.io/ksX3Vp.gif)

## FAQ

#### Does this work on every mmoga region?

I created this explicitly for mmoga.(DE) I don't know about mmoga.com or other regions in general, but I am working on including them.

#### Why can't I obtain data X?

I'm trying my best, so please be patient or include the things you like to see yourself or contact me :)

#### Is it working?

Right now in end of July '22, this tool is working pretty nice!

## License

**Software:** MMOGA Product Scraper (MMOGA-PS)

**License:** GNU General Public License v3.0

**Licensor:** Kim Mario Liebl

[GNU](https://choosealicense.com/licenses/gpl-3.0/)

## Roadmap

#### Sorted by importancy
- Include more pages than just the first page of the search (Almost done, WIP)
- Option to change region of mmoga (WIP)
- Create more objects for things, like payment ways etc.
- Parse similar products on a products site
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