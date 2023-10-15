using System.Text.Json.Serialization;
using SteamWebWrapper.Contracts.Converters;

namespace SteamWebWrapper.Contracts.Entities.Market.History;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MarketHistoryPurchase
{
    [JsonPropertyName("listingid")]
    public long ListingId { get; set; }

    [JsonPropertyName("purchaseid")]
    public long PurchaseId { get; set; }

    [JsonPropertyName("time_sold")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TimeSold { get; set; }

    [JsonPropertyName("steamid_purchaser")]
    public long SteamIdPurchaser { get; set; }

    [JsonPropertyName("needs_rollback")]
    [JsonConverter(typeof(BoolConverter))]
    public bool NeedsRollback { get; set; }

    [JsonPropertyName("failed")]
    [JsonConverter(typeof(BoolConverter))]
    public bool Failed { get; set; }

    [JsonPropertyName("asset")]
    public MarketHistoryPurchaseAsset Asset { get; set; }

    [JsonPropertyName("paid_amount")]
    public long PaidAmount { get; set; }

    [JsonPropertyName("paid_fee")]
    public long PaidFee { get; set; }

    [JsonPropertyName("currencyid")]
    public long CurrencyId { get; set; }

    [JsonPropertyName("steam_fee")]
    public long SteamFee { get; set; }

    [JsonPropertyName("publisher_fee")]
    public long PublisherFee { get; set; }

    [JsonPropertyName("publisher_fee_percent")]
    public float PublisherFeePercent { get; set; }

    [JsonPropertyName("publisher_fee_app")]
    public long PublisherFeeApp { get; set; }

    [JsonPropertyName("received_amount")]
    public long ReceivedAmount { get; set; }

    [JsonPropertyName("received_currencyid")]
    public long ReceivedCurrencyId { get; set; }

    [JsonPropertyName("funds_returned")]
    public long FundsReturned { get; set; }

    [JsonPropertyName("avatar_actor")]
    public string AvatarActor { get; set; }

    [JsonPropertyName("persona_actor")]
    public string PersonaActor { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonConverter(typeof(BoolConverter))]
    [JsonPropertyName("funds_held")]
    public bool? FundsHeld { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("time_funds_held_until")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? TimeFundsHeldUntil { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("funds_revoked")]
    [JsonConverter(typeof(BoolConverter))]
    public bool? FundsRevoked { get; set; }
}