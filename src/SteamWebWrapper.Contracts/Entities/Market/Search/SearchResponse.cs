using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Market.Search;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class SearchResponse
{
	[JsonPropertyName("pagesize")] public long PageSize { get; set; }

	[JsonPropertyName("searchdata")] public SearchData SearchData { get; set; }

	[JsonPropertyName("results")] public List<SearchMarketItem> SearchResult { get; set; }

	[JsonPropertyName("start")] public long Start { get; set; }

	[JsonPropertyName("success")] public bool Success { get; set; }

	[JsonPropertyName("total_count")] public long TotalCount { get; set; }
}