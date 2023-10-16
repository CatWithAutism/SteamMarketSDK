namespace SteamWebWrapper.Contracts.Entities.Market.CreateBuyOrder;

public class CreateBuyOrderRequest
{
    public required long Currency { get; set; }
    
    public required long AppId { get; set; }
    
    public required string MarketHashName { get; set; }
    
    public required long TotalPrice { get; set; }
    
    public required long Quantity { get; set; }
}