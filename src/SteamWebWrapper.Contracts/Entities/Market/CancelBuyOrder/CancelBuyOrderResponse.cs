using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Market.CancelBuyOrder;

public class CancelBuyOrderResponse
{
    [JsonPropertyName("success")]
    public long Success { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}