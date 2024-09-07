namespace SteamMarketSDK.Contracts.Entities.Market.PriceHistory;

public class PriceHistoryPeriodPrice
{
	public required DateTime Period { get; set; }
	public required float Price { get; set; }
	public required long Quantity { get; set; }
}