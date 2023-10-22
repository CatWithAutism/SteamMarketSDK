using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Market.MyListings;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MyListingsListing
{
    [JsonPropertyName("listingid")]
    public long ListingId { get; set; }

    [JsonPropertyName("time_created")]
    public long TimeCreated { get; set; }

    [JsonPropertyName("asset")]
    public MyListingsAsset MyListingsAsset { get; set; }

    [JsonPropertyName("steamid_lister")]
    public long SteamIdLister { get; set; }

    [JsonPropertyName("price")]
    public long Price { get; set; }

    [JsonPropertyName("original_price")]
    public long OriginalPrice { get; set; }

    [JsonPropertyName("fee")]
    public long Fee { get; set; }

    [JsonPropertyName("currencyid")]
    public long CurrencyId { get; set; }

    [JsonPropertyName("converted_price")]
    public long ConvertedPrice { get; set; }

    [JsonPropertyName("converted_fee")]
    public long ConvertedFee { get; set; }

    [JsonPropertyName("converted_currencyid")]
    public long ConvertedCurrencyId { get; set; }

    [JsonPropertyName("status")]
    public long Status { get; set; }

    [JsonPropertyName("active")]
    public long Active { get; set; }

    [JsonPropertyName("steam_fee")]
    public long SteamFee { get; set; }

    [JsonPropertyName("converted_steam_fee")]
    public long ConvertedSteamFee { get; set; }

    [JsonPropertyName("publisher_fee")]
    public long PublisherFee { get; set; }

    [JsonPropertyName("converted_publisher_fee")]
    public long ConvertedPublisherFee { get; set; }

    [JsonPropertyName("publisher_fee_percent")]
    public string PublisherFeePercent { get; set; }

    [JsonPropertyName("publisher_fee_app")]
    public long PublisherFeeApp { get; set; }

    [JsonPropertyName("cancel_reason")]
    public long CancelReason { get; set; }

    [JsonPropertyName("item_expired")]
    public long ItemExpired { get; set; }

    [JsonPropertyName("original_amount_listed")]
    public long OriginalAmountListed { get; set; }

    [JsonPropertyName("original_price_per_unit")]
    public long OriginalPricePerUnit { get; set; }

    [JsonPropertyName("fee_per_unit")]
    public long FeePerUnit { get; set; }

    [JsonPropertyName("steam_fee_per_unit")]
    public long SteamFeePerUnit { get; set; }

    [JsonPropertyName("publisher_fee_per_unit")]
    public long PublisherFeePerUnit { get; set; }

    [JsonPropertyName("converted_price_per_unit")]
    public long ConvertedPricePerUnit { get; set; }

    [JsonPropertyName("converted_fee_per_unit")]
    public long ConvertedFeePerUnit { get; set; }

    [JsonPropertyName("converted_steam_fee_per_unit")]
    public long ConvertedSteamFeePerUnit { get; set; }

    [JsonPropertyName("converted_publisher_fee_per_unit")]
    public long ConvertedPublisherFeePerUnit { get; set; }

    [JsonPropertyName("time_finish_hold")]
    public long TimeFinishHold { get; set; }

    [JsonPropertyName("time_created_str")]
    public string TimeCreatedStr { get; set; }
}