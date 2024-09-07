using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Market.Search;

public class FilterCategory
{
	[JsonPropertyName("appid")] public long AppId { get; set; }

	[JsonPropertyName("tags")] public required Dictionary<string, FilterItem> FilterItems { get; set; }

	[JsonPropertyName("localized_name")] public required string LocalizedName { get; set; }

	[JsonPropertyName("name")] public required string Name { get; set; }
}