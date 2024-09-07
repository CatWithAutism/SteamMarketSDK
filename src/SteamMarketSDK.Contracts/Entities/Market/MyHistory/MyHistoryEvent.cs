using SteamMarketSDK.Contracts.Converters;
using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Market.MyHistory;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MyHistoryEvent
{
	[JsonPropertyName("date_event")] public string DateEvent { get; set; }

	[JsonPropertyName("event_type")] public MyHistoryEventType EventType { get; set; }

	[JsonPropertyName("listingid")] public long ListingId { get; set; }

	[JsonPropertyName("purchaseid")] public long PurchaseId { get; set; }

	[JsonPropertyName("steamid_actor")] public long SteamIdActor { get; set; }

	[JsonPropertyName("time_event")]
	[JsonConverter(typeof(TimestampToDateConverter))]
	public DateTime TimeEvent { get; set; }

	[JsonPropertyName("time_event_fraction")]
	public long TimeEventFraction { get; set; }
}