using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Market.MyHistory;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MyHistoryPurchaseAsset
{
	[JsonPropertyName("amount")] public long Amount { get; set; }

	[JsonPropertyName("appid")] public long AppId { get; set; }

	[JsonPropertyName("classid")] public long ClassId { get; set; }

	[JsonPropertyName("contextid")] public long ContextId { get; set; }

	[JsonPropertyName("currency")] public long Currency { get; set; }

	[JsonPropertyName("id")] public long Id { get; set; }

	[JsonPropertyName("instanceid")] public long InstanceId { get; set; }

	[JsonPropertyName("new_contextid")] public long NewContextId { get; set; }

	[JsonPropertyName("new_id")] public long NewId { get; set; }

	[JsonPropertyName("status")] public long Status { get; set; }
}