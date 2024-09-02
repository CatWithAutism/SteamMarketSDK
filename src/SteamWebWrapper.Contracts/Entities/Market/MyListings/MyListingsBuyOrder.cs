using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Market.MyListings;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MyListingsBuyOrder
{
	[JsonPropertyName("appid")] public long AppId { get; set; }

	[JsonPropertyName("buy_orderid")] public long BuyOrderId { get; set; }

	[JsonPropertyName("description")] public MyListingsBuyOrderDescription Description { get; set; }

	[JsonPropertyName("hash_name")] public string HashName { get; set; }

	[JsonPropertyName("price")] public long Price { get; set; }

	[JsonPropertyName("quantity")] public long Quantity { get; set; }

	[JsonPropertyName("quantity_remaining")]
	public long QuantityRemaining { get; set; }

	[JsonPropertyName("wallet_currency")] public long WalletCurrency { get; set; }
}