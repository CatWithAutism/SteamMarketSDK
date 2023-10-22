using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using SteamWebWrapper.Contracts.Entities.Account;
using SteamWebWrapper.Contracts.Entities.Market.BuyOrderStatus;
using SteamWebWrapper.Contracts.Entities.Market.MyHistory;
using SteamWebWrapper.Contracts.Entities.Market.MyListings;
using SteamWebWrapper.Core.Interfaces;
using SteamWebWrapper.Interfaces;

namespace SteamWebWrapper.Implementations;

public class MarketWrapper : IMarketWrapper
{
    private readonly ISteamHttpClient _steamHttpClient;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="steamHttpClient">Your steam client should be already authorized otherwise you can get errors.</param>
    public MarketWrapper(ISteamHttpClient steamHttpClient)
    {
        _steamHttpClient = steamHttpClient;
    }
    
    public async Task<MyHistoryResponse?> GetMarketHistoryAsync(long offset, long count, CancellationToken cancellationToken)
    {
        var requestUri = $"https://steamcommunity.com/market/myhistory/?query=&count={count}&start={offset}&norender=true";
        
        var response = await _steamHttpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<MyHistoryResponse>(stringResponse);
    }
    
    public async Task<AccountInfo?> CollectMarketAccountInfo(CancellationToken cancellationToken)
    {
        const string infoPage = "https://steamcommunity.com/market/#";
        
        var response = await _steamHttpClient.GetAsync(infoPage, cancellationToken);
        response.EnsureSuccessStatusCode();

        var webPage = await response.Content.ReadAsStringAsync(cancellationToken);
        var match = Regex.Match(webPage, @"{\s*\""wallet_currency\""[A-Za-z0-9:\.\s,\""\\_\-]+}");

        return JsonSerializer.Deserialize<AccountInfo>(match.Value);
    }

    /// <summary>
    /// Returns current listings and buy orders.
    /// </summary>
    /// <param name="offset">Offset from zero element. Than more than older.</param>
    /// <param name="count">Count of elements. Max size is 500</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public async Task<MyListingsResponse?> GetMyListings(long offset, long count, CancellationToken cancellationToken)
    {
        string requestUri = $"https://steamcommunity.com/market/mylistings?count={count}&start={offset}&norender=true";
        
        var response = await _steamHttpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<MyListingsResponse>(stringResponse);
    }
    
    public async Task<BuyOrderStatusResponse?> GetBuyOrderStatus(long buyOrderId, CancellationToken cancellationToken)
    {
        string requestUri = $"https://steamcommunity.com/market/getbuyorderstatus?sessionid={_steamHttpClient.SessionId}&buy_orderid={buyOrderId}";

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(requestUri),
            Method = HttpMethod.Get,
        };
        request.Headers.Referrer = new Uri($"https://steamcommunity.com/id/{_steamHttpClient.SteamId}/inventory/");
        
        var response = await _steamHttpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<BuyOrderStatusResponse>(stringResponse);
    }
    
    public async Task<BuyOrderStatusResponse?> GetCurrentPrice(long buyOrderId, CancellationToken cancellationToken)
    {
        string requestUri = $"https://steamcommunity.com/market/getbuyorderstatus?sessionid={_steamHttpClient.SessionId}&buy_orderid={buyOrderId}";

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(requestUri),
            Method = HttpMethod.Get,
        };
        request.Headers.Referrer = new Uri($"https://steamcommunity.com/id/{_steamHttpClient.SteamId}/inventory/");
        
        var response = await _steamHttpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<BuyOrderStatusResponse>(stringResponse);
    }

    public void Dispose()
    {
        _steamHttpClient.Dispose();
    }
}