using SteamMarketSDK.Contracts.Converters;
using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Market.MyHistory;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MyHistoryResponse
{
	[JsonPropertyName("assets")]
	[JsonConverter(typeof(MyHistoryAssetFlatter))]
	public List<MyHistoryAsset> Assets { get; set; }

	[JsonPropertyName("events")] public List<MyHistoryEvent> Events { get; set; }

	[JsonPropertyName("listings")]
	[JsonConverter(typeof(ListFromDictionaryConverter<MyHistoryListing>))]
	public List<MyHistoryListing> Listings { get; set; }

	[JsonPropertyName("pagesize")] public long PageSize { get; set; }

	[JsonPropertyName("purchases")]
	[JsonConverter(typeof(ListFromDictionaryConverter<MyHistoryPurchase>))]
	public List<MyHistoryPurchase> Purchases { get; set; }

	[JsonPropertyName("start")] public long Start { get; set; }

	[JsonPropertyName("success")] public bool Success { get; set; }

	[JsonPropertyName("total_count")] public long TotalCount { get; set; }
}