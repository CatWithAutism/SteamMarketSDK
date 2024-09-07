using SteamMarketSDK.Contracts.Converters;
using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Market.MyListings;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class MyListingsResponse
{
	[JsonPropertyName("assets")]
	[JsonConverter(typeof(MyListingsAssetFlatter))]
	public List<MyListingsAsset> Assets { get; set; }

	[JsonPropertyName("buy_orders")] public List<MyListingsBuyOrder> BuyOrders { get; set; }

	[JsonPropertyName("listings")] public List<MyListingsListing> Listings { get; set; }

	[JsonPropertyName("listings_on_hold")] public List<dynamic> ListingsOnHold { get; set; }

	[JsonPropertyName("listings_to_confirm")]
	public List<dynamic> ListingsToConfirm { get; set; }

	[JsonPropertyName("num_active_listings")]
	public long NumActiveListings { get; set; }

	[JsonPropertyName("pagesize")] public long PageSize { get; set; }

	[JsonPropertyName("start")] public long Start { get; set; }

	[JsonPropertyName("success")] public bool Success { get; set; }

	[JsonPropertyName("total_count")] public long TotalCount { get; set; }
}