using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Market.BuyOrderStatus;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class BuyOrderStatusPurchase
{
    [JsonPropertyName("listingid")]
    public string ListingId { get; set; }

    [JsonPropertyName("price_subtotal")]
    public long PriceSubtotal { get; set; }

    [JsonPropertyName("price_fee")]
    public long PriceFee { get; set; }

    [JsonPropertyName("price_total")]
    public long PriceTotal { get; set; }

    [JsonPropertyName("currency")]
    public long Currency { get; set; }

    [JsonPropertyName("appid")]
    public long Appid { get; set; }

    [JsonPropertyName("assetid")]
    public string AssetId { get; set; }

    [JsonPropertyName("contextid")]
    public long ContextId { get; set; }

    [JsonPropertyName("accountid_seller")]
    public long AccountIdSeller { get; set; }
}