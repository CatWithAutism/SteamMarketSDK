using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using SteamWebWrapper.Contracts.Entities.Market.AccountInfo;
using SteamWebWrapper.Contracts.Entities.Market.BuyOrderStatus;
using SteamWebWrapper.Contracts.Entities.Market.CancelBuyOrder;
using SteamWebWrapper.Contracts.Entities.Market.CreateBuyOrder;
using SteamWebWrapper.Contracts.Entities.Market.MyHistory;
using SteamWebWrapper.Contracts.Entities.Market.MyListings;
using SteamWebWrapper.Contracts.Entities.Market.PriceOverview;
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
    
    public async Task<AccountInfoResponse?> CollectMarketAccountInfo(CancellationToken cancellationToken)
    {
        const string infoPage = "https://steamcommunity.com/market/#";
        
        var response = await _steamHttpClient.GetAsync(infoPage, cancellationToken);
        response.EnsureSuccessStatusCode();

        var webPage = await response.Content.ReadAsStringAsync(cancellationToken);
        
        var match = Regex.Match(webPage, @"{\s*\""wallet_currency\""[A-Za-z0-9:\.\s,\""\\_\-]+}");
        var accountInfo = JsonSerializer.Deserialize<AccountInfoResponse>(match.Value);
        
        match = Regex.Match(webPage, @"dateCanUseMarket\s*=\s*new\s*Date\(\""(.+?)\""\)");
        if (match.Success)
        {
            accountInfo.MarketAllowed = false;
            accountInfo.DateCanUseMarket = DateTime.Parse(match.Groups[1].Value);
        }
        else
        {
            accountInfo.MarketAllowed = true;
        }

        return accountInfo;
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

    /// <summary>
    /// Request prive overwive of specified item.
    /// </summary>
    /// <param name="priceRequest">Price request.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Price overview response</returns>
    public async Task<PriceOverviewResponse?> GetItemCurrentPrice(PriceOverviewRequest priceRequest, CancellationToken cancellationToken)
    {
        string requestUri = $"https://steamcommunity.com/market/priceoverview/?" +
                            $"country={priceRequest.Country}&" +
                            $"currency={priceRequest.Currency}&" +
                            $"appid={priceRequest.AppId}&" +
                            $"market_hash_name={HttpUtility.UrlEncode(priceRequest.MarketHashName)}";
        
        var response = await _steamHttpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<PriceOverviewResponse>(stringResponse);
    }

    /// <summary>
    /// Request to create buy order.
    /// </summary>
    /// <param name="createBuyOrderRequest">Request.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task<CreateBuyOrderResponse?> CreateBuyOrder(CreateBuyOrderRequest createBuyOrderRequest, CancellationToken cancellationToken)
    {
        const string requestUri = $"https://steamcommunity.com/market/createbuyorder/";
        string urlEncodedHashName = HttpUtility.UrlEncode(createBuyOrderRequest.MarketHashName); 
        
        var content = new FormUrlEncodedContent(new []
        {
            new KeyValuePair<string, string>("sessionid", _steamHttpClient.SessionId),
            new KeyValuePair<string, string>("currency", createBuyOrderRequest.Currency.ToString()),
            new KeyValuePair<string, string>("appid", createBuyOrderRequest.AppId.ToString()),
            new KeyValuePair<string, string>("market_hash_name", createBuyOrderRequest.MarketHashName),
            new KeyValuePair<string, string>("price_total", createBuyOrderRequest.PriceTotal.ToString()),
            new KeyValuePair<string, string>("quantity", createBuyOrderRequest.Quantity.ToString()),
            new KeyValuePair<string, string>("billing_state", string.Empty),
            new KeyValuePair<string, string>("save_my_address", "0"),
        });
        
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(requestUri),
            Method = HttpMethod.Post,
            Content = content,
        };
        request.Headers.Referrer = new Uri($"https://steamcommunity.com/market/listings/730/{urlEncodedHashName}");
        
        var response = await _steamHttpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<CreateBuyOrderResponse>(stringResponse);
    }

    /// <summary>
    /// Request to cancel buy order.
    /// </summary>
    /// <param name="buyOrderId">Buy order id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task<CancelBuyOrderResponse?> CancelBuyOrder(long buyOrderId, CancellationToken cancellationToken)
    {
        const string requestUri = $"https://steamcommunity.com/market/cancelbuyorder/";
        
        var content = new FormUrlEncodedContent(new []
        {
            new KeyValuePair<string, string>("sessionid", _steamHttpClient.SessionId),
            new KeyValuePair<string, string>("buy_orderid", buyOrderId.ToString()),
        });
        
        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(requestUri),
            Method = HttpMethod.Post,
            Content = content,
        };
        
        request.Headers.Referrer = new Uri($"https://steamcommunity.com/market/");
        
        var response = await _steamHttpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<CancelBuyOrderResponse>(stringResponse);
    }

    public void Dispose()
    {
        _steamHttpClient.Dispose();
    }
}