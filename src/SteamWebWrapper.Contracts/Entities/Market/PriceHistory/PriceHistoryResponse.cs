using System.Text.Json.Serialization;
using SteamWebWrapper.Contracts.Converters;

namespace SteamWebWrapper.Contracts.Entities.Market.PriceHistory;

public class PriceHistoryResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("price_prefix")]
    public string PricePrefix { get; set; }

    [JsonPropertyName("price_suffix")]
    public string PriceSuffix { get; set; }

    [JsonPropertyName("prices")]
    [JsonConverter(typeof(PeriodPriceConverter))]
    public List<PriceHistoryPeriodPrice> PeriodPrices { get; set; }
}