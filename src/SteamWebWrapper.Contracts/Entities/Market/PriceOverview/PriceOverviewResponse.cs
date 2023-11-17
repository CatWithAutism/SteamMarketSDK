using System.Text.Json.Serialization;
using SteamWebWrapper.Contracts.Converters;

namespace SteamWebWrapper.Contracts.Entities.Market.PriceOverview;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class PriceOverviewResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("lowest_price")]
    [JsonConverter(typeof(PriceConverter))]
    public float LowestPrice { get; set; }

    [JsonPropertyName("volume")]
    [JsonConverter(typeof(VolumeConverter))]
    public long Volume { get; set; }

    [JsonPropertyName("median_price")]
    [JsonConverter(typeof(PriceConverter))]
    public float MedianPrice { get; set; }
}