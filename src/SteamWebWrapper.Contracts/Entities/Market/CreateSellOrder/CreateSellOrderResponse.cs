using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Market.CreateSellOrder;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class CreateSellOrderResponse
{
	[JsonPropertyName("message")] public string? Message { get; set; }

	[JsonPropertyName("requires_confirmation")]
	public int? RequiresConfirmation { get; set; }

	[JsonPropertyName("success")] public bool Success { get; set; }
}