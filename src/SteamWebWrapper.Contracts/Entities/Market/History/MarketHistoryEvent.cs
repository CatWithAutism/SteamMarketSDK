using System.Text.Json.Serialization;
using SteamWebWrapper.Contracts.Converters;

namespace SteamWebWrapper.Contracts.Entities.Market.History;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MarketHistoryEvent
{
    [JsonPropertyName("listingid")]
    public long ListingId { get; set; }

    [JsonPropertyName("event_type")]
    public MarketHistoryEventType EventType { get; set; }

    [JsonPropertyName("time_event")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TimeEvent { get; set; }

    [JsonPropertyName("time_event_fraction")]
    public long TimeEventFraction { get; set; }

    [JsonPropertyName("steamid_actor")]
    public long SteamIdActor { get; set; }

    [JsonPropertyName("date_event")]
    public string DateEvent { get; set; }
    
    [JsonPropertyName("purchaseid")]
    public long PurchaseId { get; set; }
}