using System.Text.Json.Serialization;
using SteamWebWrapper.Contracts.Entities.Common;

namespace SteamWebWrapper.Contracts.Entities.Market.MyHistory;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MyHistoryListingAsset
{
    [JsonPropertyName("currency")]
    public long Currency { get; set; }

    [JsonPropertyName("appid")]
    public long AppId { get; set; }

    [JsonPropertyName("contextid")]
    public long ContextId { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("market_actions")]
    public List<SubjectAction> MarketActions { get; set; }
}