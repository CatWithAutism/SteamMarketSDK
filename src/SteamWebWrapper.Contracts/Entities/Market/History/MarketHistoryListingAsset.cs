using System.Text.Json.Serialization;
using SteamWebWrapper.Contracts.Entities.Common;

namespace SteamWebWrapper.Contracts.Entities.Market.History;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MarketHistoryListingAsset
{
    [JsonPropertyName("currency")]
    public long Currency { get; set; }

    [JsonPropertyName("appid")]
    public long Appid { get; set; }

    [JsonPropertyName("contextid")]
    public long ContextId { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("market_actions")]
    public List<SubjectAction> MarketActions { get; set; }
}