using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Market.CancelBuyOrder;

public class CancelBuyOrderResponse
{
	[JsonPropertyName("error")] public string? Error { get; set; }

	[JsonPropertyName("success")] public long Success { get; set; }
}