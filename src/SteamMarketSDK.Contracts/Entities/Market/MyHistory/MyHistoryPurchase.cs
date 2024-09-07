using SteamMarketSDK.Contracts.Converters;
using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Market.MyHistory;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MyHistoryPurchase
{
	[JsonPropertyName("asset")] public MyHistoryPurchaseAsset Asset { get; set; }

	[JsonPropertyName("avatar_actor")] public string AvatarActor { get; set; }

	[JsonPropertyName("currencyid")] public long CurrencyId { get; set; }

	[JsonPropertyName("failed")] public long Failed { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	[JsonPropertyName("funds_held")]
	public long? FundsHeld { get; set; }

	[JsonPropertyName("funds_returned")] public long FundsReturned { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	[JsonPropertyName("funds_revoked")]
	public long? FundsRevoked { get; set; }

	[JsonPropertyName("listingid")] public long ListingId { get; set; }

	[JsonPropertyName("needs_rollback")] public long NeedsRollback { get; set; }

	[JsonPropertyName("paid_amount")] public long PaidAmount { get; set; }

	[JsonPropertyName("paid_fee")] public long PaidFee { get; set; }

	[JsonPropertyName("persona_actor")] public string PersonaActor { get; set; }

	[JsonPropertyName("publisher_fee")] public long PublisherFee { get; set; }

	[JsonPropertyName("publisher_fee_app")]
	public long PublisherFeeApp { get; set; }

	[JsonPropertyName("publisher_fee_percent")]
	public float PublisherFeePercent { get; set; }

	[JsonPropertyName("purchaseid")] public long PurchaseId { get; set; }

	[JsonPropertyName("received_amount")] public long ReceivedAmount { get; set; }

	[JsonPropertyName("received_currencyid")]
	public long ReceivedCurrencyId { get; set; }

	[JsonPropertyName("steam_fee")] public long SteamFee { get; set; }

	[JsonPropertyName("steamid_purchaser")]
	public long SteamIdPurchaser { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	[JsonPropertyName("time_funds_held_until")]
	[JsonConverter(typeof(TimestampToDateConverter))]
	public DateTime? TimeFundsHeldUntil { get; set; }

	[JsonPropertyName("time_sold")]
	[JsonConverter(typeof(TimestampToDateConverter))]
	public DateTime TimeSold { get; set; }
}