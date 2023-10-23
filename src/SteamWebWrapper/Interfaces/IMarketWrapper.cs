using SteamWebWrapper.Contracts.Entities.Market.AccountInfo;
using SteamWebWrapper.Contracts.Entities.Market.BuyOrderStatus;
using SteamWebWrapper.Contracts.Entities.Market.CancelBuyOrder;
using SteamWebWrapper.Contracts.Entities.Market.CreateBuyOrder;
using SteamWebWrapper.Contracts.Entities.Market.MyHistory;
using SteamWebWrapper.Contracts.Entities.Market.MyListings;
using SteamWebWrapper.Contracts.Entities.Market.PriceHistory;
using SteamWebWrapper.Contracts.Entities.Market.PriceOverview;

namespace SteamWebWrapper.Interfaces;

public interface IMarketWrapper : IDisposable
{
    Task<MyHistoryResponse?> GetMarketHistoryAsync(long offset, long count, CancellationToken cancellationToken);
    Task<AccountInfoResponse?> CollectMarketAccountInfo(CancellationToken cancellationToken);

    /// <summary>
    /// Returns current listings and buy orders.
    /// </summary>
    /// <param name="offset">Offset from zero element. Than more than older.</param>
    /// <param name="count">Count of elements. Max size is 500</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    Task<MyListingsResponse?> GetMyListings(long offset, long count, CancellationToken cancellationToken);

    Task<BuyOrderStatusResponse?> GetBuyOrderStatus(long buyOrderId, CancellationToken cancellationToken);

    /// <summary>
    /// Request prive overwive of specified item.
    /// </summary>
    /// <param name="priceRequest">Price request.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Price overview response</returns>
    Task<PriceOverviewResponse?> GetItemCurrentPrice(PriceOverviewRequest priceRequest, CancellationToken cancellationToken);

    /// <summary>
    /// Request to cancel buy order.
    /// </summary>
    /// <param name="buyOrderId">Buy order id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<CancelBuyOrderResponse?> CancelBuyOrder(long buyOrderId, CancellationToken cancellationToken);

    /// <summary>
    /// Request to create buy order.
    /// </summary>
    /// <param name="createBuyOrderRequest">Request.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<CreateBuyOrderResponse?> CreateBuyOrder(CreateBuyOrderRequest createBuyOrderRequest, CancellationToken cancellationToken);

    /// <summary>
    /// Returns price history of the market item.
    /// </summary>
    /// <param name="appId">Game id</param>
    /// <param name="marketHashName">Market hash name.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    Task<PriceHistoryResponse?> GetPriceHistory(long appId, string marketHashName, CancellationToken cancellationToken);
}