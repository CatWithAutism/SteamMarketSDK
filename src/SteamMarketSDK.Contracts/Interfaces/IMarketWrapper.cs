using SteamMarketSDK.Contracts.Entities.Market.AccountInfo;
using SteamMarketSDK.Contracts.Entities.Market.BuyOrderStatus;
using SteamMarketSDK.Contracts.Entities.Market.CancelBuyOrder;
using SteamMarketSDK.Contracts.Entities.Market.CreateBuyOrder;
using SteamMarketSDK.Contracts.Entities.Market.CreateSellOrder;
using SteamMarketSDK.Contracts.Entities.Market.ItemOrdersActivity;
using SteamMarketSDK.Contracts.Entities.Market.MyHistory;
using SteamMarketSDK.Contracts.Entities.Market.MyListings;
using SteamMarketSDK.Contracts.Entities.Market.PriceHistory;
using SteamMarketSDK.Contracts.Entities.Market.PriceOverview;
using SteamMarketSDK.Contracts.Entities.Market.Search;

namespace SteamMarketSDK.Contracts.Interfaces;

public interface IMarketWrapper : IDisposable
{
	/// <summary>
	///     Request to cancel buy order.
	/// </summary>
	/// <param name="buyOrderId">Buy order id</param>
	/// <param name="cancellationToken">Cancellation token</param>
	Task<CancelBuyOrderResponse> CancelBuyOrderAsync(long buyOrderId, CancellationToken cancellationToken);

	/// <summary>
	///     Request to create sell order.
	/// </summary>
	Task<bool> CancelSellOrderAsync(long listingId, CancellationToken cancellationToken);

	/// <summary>
	///     Request to create buy order.
	/// </summary>
	/// <param name="createBuyOrderRequest">Request.</param>
	/// <param name="cancellationToken">Cancellation token</param>
	Task<CreateBuyOrderResponse> CreateBuyOrderAsync(CreateBuyOrderRequest createBuyOrderRequest,
		CancellationToken cancellationToken);

	/// <summary>
	///     Request to create sell order.
	/// </summary>
	Task<CreateSellOrderResponse> CreateSellOrderAsync(CreateSellOrderRequest createSellOrderRequest,
		CancellationToken cancellationToken);

	Task<AccountInfoResponse> GetAccountInfoAsync(CancellationToken cancellationToken);

	Task<BuyOrderStatusResponse> GetBuyOrderStatusAsync(long buyOrderId, CancellationToken cancellationToken);

	/// <summary>
	///     Fetch item name id of specified item
	/// </summary>
	/// <param name="appId">AppId of Steam</param>
	/// <param name="marketHashName">Market Hash Name</param>
	Task<long> GetItemNameIdAsync(long appId, string marketHashName, CancellationToken cancellationToken);

	Task<ItemOrdersActivityResponse> GetItemOrdersActivityAsync(long itemNameId, long currency,
		string language = "english", string country = "US", CancellationToken cancellationToken = default);

	/// <summary>
	///     Returns current listings and buy orders.
	/// </summary>
	/// <param name="offset">Offset from zero element. Than more than older.</param>
	/// <param name="count">Count of elements. Max size is 500</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns></returns>
	Task<MyListingsResponse> GetListingsAsync(long offset, long count, CancellationToken cancellationToken);

	/// <summary>
	///     Returns price history of the market item.
	/// </summary>
	/// <param name="appId">Game id</param>
	/// <param name="marketHashName">Market hash name.</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns></returns>
	Task<PriceHistoryResponse> GetPriceHistoryAsync(long appId, string marketHashName,
		CancellationToken cancellationToken);

	/// <summary>
	///     Request price overview of specified item.
	/// </summary>
	/// <param name="priceRequest">Price request.</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Price overview response</returns>
	Task<PriceOverviewResponse> GetPriceOverviewAsync(PriceOverviewRequest priceRequest,
		CancellationToken cancellationToken);

	Task<MyHistoryResponse> GetTradeHistoryAsync(long offset, long count, CancellationToken cancellationToken);

	/// <summary>
	///     Do not use it. This method raw as fuck.
	/// </summary>
	Task<SearchResponse> SearchItemsAsync(string? query, long offset, long count, string searchData,
		CancellationToken cancellationToken);
}