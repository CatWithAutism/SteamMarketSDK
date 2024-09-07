namespace SteamMarketSDK.Contracts.Entities.Market.CreateSellOrder;

public class CreateSellOrderRequest
{
	public CreateSellOrderRequest(long appId, long contextId, long assetId, long price, long quantity)
	{
		AppId = appId;
		ContextId = contextId;
		AssetId = assetId;
		Price = price;
		Quantity = quantity;
	}

	public long AppId { get; private set; }
	public long AssetId { get; private set; }
	public long ContextId { get; private set; }
	public long Price { get; private set; }
	public long Quantity { get; private set; }
}