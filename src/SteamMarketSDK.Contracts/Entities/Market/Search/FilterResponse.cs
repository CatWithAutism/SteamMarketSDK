using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Market.Search;

public class FilterResponse
{
	[JsonPropertyName("facets")] public required FilterCategory FilterCategory { get; set; }

	[JsonPropertyName("success")] public required bool Success { get; set; }
}