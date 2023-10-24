using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Market.Search;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class SearchMarketItem
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("hash_name")]
    public string HashName { get; set; }

    [JsonPropertyName("sell_listings")]
    public long SellListings { get; set; }

    [JsonPropertyName("sell_price")]
    public long SellPrice { get; set; }

    [JsonPropertyName("sell_price_text")]
    public string SellPriceText { get; set; }

    [JsonPropertyName("app_icon")]
    public Uri AppIcon { get; set; }

    [JsonPropertyName("app_name")]
    public string AppName { get; set; }

    [JsonPropertyName("asset_description")]
    public SearchMarketItemDescription Description { get; set; }

    [JsonPropertyName("sale_price_text")]
    public string SalePriceText { get; set; }
}