using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Market.CreateBuyOrder;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class CreateBuyOrderResponse
{
    [JsonPropertyName("success")]
    public long Success { get; set; }

    [JsonPropertyName("buy_orderid")]
    public long BuyOrderId { get; set; }
    
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}