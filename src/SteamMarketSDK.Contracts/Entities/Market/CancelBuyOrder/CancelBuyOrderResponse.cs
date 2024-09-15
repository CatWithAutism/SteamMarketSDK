using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Market.CancelBuyOrder;

public class CancelBuyOrderResponse
{
	[JsonPropertyName("error")] public string? Error { get; set; }

	[JsonPropertyName("success")] public long Success { get; set; }
}