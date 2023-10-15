using System.Text.Json.Serialization;
using SteamWebWrapper.Contracts.Converters;

namespace SteamWebWrapper.Contracts.Entities.Market.CreateBuyOrder;

public class CreateBuyOrderResponse
{
    [JsonPropertyName("success")]
    public long Success { get; set; }

    [JsonPropertyName("buy_orderid")]
    public string BuyOrderId { get; set; }
    
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}