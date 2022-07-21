# MMOGA-PS - MMOGA Product Scraper (API)

##### Projekt von https://github.com/liebki

A scraper to get all products including their (possible) data from https://mmoga.de

## Technologies

### Created using
- .NET Core 6.0

### Nuget(s)
- HtmlAgilityPack

## Features

### What data is available right now?
- Following data is available: 
	- product type, link of product, title, price, is the price reduced, the reduced price, platform-logo, delivery time, availability, region, platform, description, is paypal available 

### General
- Get a "Product" object, it may be a game, console game, DLC, giftcard or a random item/key

## Usage

## Example

```
Code will follow, right now it is more like a tool.
Goal is to create a kind of API or something to access.
```

## FAQ

#### Does this work on every mmoga region?

I created this for mmoga.DE I don't know about mmoga.com or other regions in general.

#### Why can't I obtain data XY??

I'm trying my best, to make this a good scraper/api to allow us to scrape products of mmoga but everything needs time, so do I :)

## License

**Software:** MMOGA Product Scraper (MMOGA-PS)

**License:** GNU General Public License v3.0

**Licensor:** Kim Mario Liebl

[GNU](https://choosealicense.com/licenses/gpl-3.0/)

## Roadmap

- Make this thing a real API
- Clean up the code and split it in methods etc.
- Cover many types of products
- Learn more about parsing HTML etc. using HtmlAgilityPack