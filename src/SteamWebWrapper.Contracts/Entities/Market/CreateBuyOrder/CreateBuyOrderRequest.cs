namespace SteamWebWrapper.Contracts.Entities.Market.CreateBuyOrder;

public class CreateBuyOrderRequest
{
	public CreateBuyOrderRequest(long appId, string marketHashName, long currency, long priceTotal, long quantity)
	{
		AppId = appId;
		MarketHashName = marketHashName;
		Currency = currency;
		PriceTotal = priceTotal;
		Quantity = quantity;
	}

	public long AppId { get; private set; }
	public long Currency { get; private set; }
	public string MarketHashName { get; private set; }
	public long PriceTotal { get; private set; }
	public long Quantity { get; private set; }
}