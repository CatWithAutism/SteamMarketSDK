using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Market.BuyOrderStatus;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class BuyOrderStatusResponse
{
	[JsonPropertyName("active")] public long Active { get; set; }

	[JsonPropertyName("purchase_amount_text")]
	public string PurchaseAmountText { get; set; }

	[JsonPropertyName("purchased")] public long Purchased { get; set; }

	[JsonPropertyName("purchases")] public List<BuyOrderStatusPurchase> Purchases { get; set; }

	[JsonPropertyName("quantity")] public long Quantity { get; set; }

	[JsonPropertyName("quantity_remaining")]
	public long QuantityRemaining { get; set; }

	[JsonPropertyName("success")] public long Success { get; set; }
}