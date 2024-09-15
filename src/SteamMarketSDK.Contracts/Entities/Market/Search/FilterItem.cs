using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Market.Search;

public class FilterItem
{
	[JsonPropertyName("color")] public string? Color { get; set; }

	[JsonPropertyName("localized_name")] public required string LocalizedName { get; set; }

	[JsonPropertyName("matches")] public required string Matches { get; set; }
}