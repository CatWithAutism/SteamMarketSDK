using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Market.Search;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class SearchData
{
	[JsonPropertyName("class_prefix")] public string ClassPrefix { get; set; }

	[JsonPropertyName("pagesize")] public long PageSize { get; set; }

	[JsonPropertyName("prefix")] public string Prefix { get; set; }

	[JsonPropertyName("query")] public string Query { get; set; }

	[JsonPropertyName("search_descriptions")]
	public bool SearchDescriptions { get; set; }

	[JsonPropertyName("total_count")] public long TotalCount { get; set; }
}