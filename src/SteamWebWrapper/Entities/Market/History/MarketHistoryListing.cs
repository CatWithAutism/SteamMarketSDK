using System.Text.Json.Serialization;

namespace SteamWebWrapper.Entities.Market.History;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MarketHistoryListing
{
    [JsonPropertyName("listingid")]
    public long ListingId { get; set; }

    [JsonPropertyName("price")]
    public long Price { get; set; }

    [JsonPropertyName("fee")]
    public long Fee { get; set; }

    [JsonPropertyName("publisher_fee_app")]
    public long PublisherFeeApp { get; set; }

    [JsonPropertyName("publisher_fee_percent")]
    public float PublisherFeePercent { get; set; }

    [JsonPropertyName("currencyid")]
    public long CurrencyId { get; set; }

    [JsonPropertyName("asset")]
    public MarketHistoryListingAsset Asset { get; set; }

    [JsonPropertyName("original_price")]
    public long OriginalPrice { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("steam_fee")]
    public long? SteamFee { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("publisher_fee")]
    public long? PublisherFee { get; set; }
}