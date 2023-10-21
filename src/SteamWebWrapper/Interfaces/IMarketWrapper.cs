using SteamWebWrapper.Contracts.Entities.Account;
using SteamWebWrapper.Contracts.Entities.Market.History;

namespace SteamWebWrapper.Interfaces;

public interface IMarketWrapper : IDisposable
{
    Task<MarketHistoryResponse?> GetMarketHistoryAsync(long offset, long count);
    Task<AccountInfo?> GetMarketAccountInfo(CancellationToken cancellationToken);
}