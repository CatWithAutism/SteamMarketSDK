using SteamWebWrapper.Contracts.Entities.Account;
using SteamWebWrapper.Contracts.Entities.Market.BuyOrderStatus;
using SteamWebWrapper.Contracts.Entities.Market.MyHistory;
using SteamWebWrapper.Contracts.Entities.Market.MyListings;
using SteamWebWrapper.Contracts.Entities.Market.PriceOverview;

namespace SteamWebWrapper.Interfaces;

public interface IMarketWrapper : IDisposable
{
    Task<MyHistoryResponse?> GetMarketHistoryAsync(long offset, long count, CancellationToken cancellationToken);
    Task<AccountInfo?> CollectMarketAccountInfo(CancellationToken cancellationToken);

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
}