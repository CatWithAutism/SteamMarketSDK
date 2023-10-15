using System.Text.Json.Serialization;
using SteamWebWrapper.Contracts.Converters;

namespace SteamWebWrapper.Contracts.Entities.Market.History;


[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MarketHistoryResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("pagesize")]
    public long PageSize { get; set; }

    [JsonPropertyName("total_count")]
    public long TotalCount { get; set; }

    [JsonPropertyName("start")]
    public long Start { get; set; }

    [JsonPropertyName("assets")]
    [JsonConverter(typeof(AssetFlatter))]
    public List<MarketHistoryAsset> Assets { get; set; }

    [JsonPropertyName("events")]
    public List<MarketHistoryEvent> Events { get; set; }

    [JsonPropertyName("purchases")]
    [JsonConverter(typeof(ListFromDictionaryConverter<MarketHistoryPurchase>))]
    public List<MarketHistoryPurchase> Purchases { get; set; }

    [JsonPropertyName("listings")]
    [JsonConverter(typeof(ListFromDictionaryConverter<MarketHistoryListing>))]
    public List<MarketHistoryListing> Listings { get; set; }
}