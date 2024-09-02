using SteamWebWrapper.Contracts.Converters;
using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Market.PriceHistory;

public class PriceHistoryResponse
{
	[JsonPropertyName("prices")]
	[JsonConverter(typeof(PeriodPriceConverter))]
	public List<PriceHistoryPeriodPrice> PeriodPrices { get; set; }

	[JsonPropertyName("price_prefix")] public string PricePrefix { get; set; }

	[JsonPropertyName("price_suffix")] public string PriceSuffix { get; set; }

	[JsonPropertyName("success")] public bool Success { get; set; }
}