using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Market.History;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MarketHistoryPurchaseAsset
{
    [JsonPropertyName("currency")]
    public long Currency { get; set; }

    [JsonPropertyName("appid")]
    public long Appid { get; set; }

    [JsonPropertyName("contextid")]
    public long ContextId { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("classid")]
    public string Classid { get; set; }

    [JsonPropertyName("instanceid")]
    public long Instanceid { get; set; }

    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    [JsonPropertyName("status")]
    public long Status { get; set; }

    [JsonPropertyName("new_id")]
    public long NewId { get; set; }

    [JsonPropertyName("new_contextid")]
    public long NewContextId { get; set; }
}