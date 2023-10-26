using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using SteamWebWrapper.Common.Utils;
using SteamWebWrapper.Contracts.Entities.Market.AccountInfo;
using SteamWebWrapper.Contracts.Entities.Market.BuyOrderStatus;
using SteamWebWrapper.Contracts.Entities.Market.CancelBuyOrder;
using SteamWebWrapper.Contracts.Entities.Market.CreateBuyOrder;
using SteamWebWrapper.Contracts.Entities.Market.CreateSellOrder;
using SteamWebWrapper.Contracts.Entities.Market.MyHistory;
using SteamWebWrapper.Contracts.Entities.Market.MyListings;
using SteamWebWrapper.Contracts.Entities.Market.PriceHistory;
using SteamWebWrapper.Contracts.Entities.Market.PriceOverview;
using SteamWebWrapper.Core.Interfaces;
using SteamWebWrapper.Interfaces;

namespace SteamWebWrapper.Implementations;

public class MarketWrapper : IMarketWrapper
{
    private ISteamHttpClient SteamHttpClient { get; }

    private string SessionId
    {
        get
        {
            var steamCommunity = new Uri("https://steamcommunity.com/");
            return SteamHttpClient.GetSessionId(steamCommunity) ??
                   throw new InvalidOperationException(
                       $"Your client is not authorized or do not have session id for domain {steamCommunity.Host}");
        }
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="steamHttpClient">Your steam client should be already authorized otherwise you can get errors.</param>
    public MarketWrapper(ISteamHttpClient steamHttpClient)
    {
        SteamHttpClient = steamHttpClient;
    }

    public async Task<MyHistoryResponse?> GetMarketHistoryAsync(long offset, long count, CancellationToken cancellationToken)
    {
        var requestUri = $"https://steamcommunity.com/market/myhistory/?query=&count={count}&start={offset}&norender=true";
        
        var response = await SteamHttpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<MyHistoryResponse>(stringResponse);
    }
    
    public async Task<AccountInfoResponse?> CollectMarketAccountInfo(CancellationToken cancellationToken)
    {
        const string infoPage = "https://steamcommunity.com/market/#";
        
        var response = await SteamHttpClient.GetAsync(infoPage, cancellationToken);
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
        
        var response = await SteamHttpClient.GetAsync(requestUri, cancellationToken);
        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        response.EnsureSuccessStatusCode();
        
        return JsonSerializer.Deserialize<MyListingsResponse>(stringResponse);
    }
    
    public async Task<BuyOrderStatusResponse?> GetBuyOrderStatus(long buyOrderId, CancellationToken cancellationToken)
    {
        string requestUri = $"https://steamcommunity.com/market/getbuyorderstatus?sessionid={SessionId}&buy_orderid={buyOrderId}";

        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(requestUri),
            Method = HttpMethod.Get,
        };
        request.Headers.Referrer = new Uri($"https://steamcommunity.com/id/{SteamHttpClient.SteamId}/inventory/");
        
        var response = await SteamHttpClient.SendAsync(request, cancellationToken);
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
        
        var response = await SteamHttpClient.GetAsync(requestUri, cancellationToken);
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
        
        
        const string requestUri = "https://steamcommunity.com/market/createbuyorder/";
        string urlEncodedHashName = HttpUtility.UrlEncode(createBuyOrderRequest.MarketHashName); 
        
        var content = new FormUrlEncodedContent(new []
        {
            new KeyValuePair<string, string>("sessionid", SessionId),
            new KeyValuePair<string, string>("currency", createBuyOrderRequest.Currency.ToString()),
            new KeyValuePair<string, string>("appid", createBuyOrderRequest.AppId.ToString()),
            new KeyValuePair<string, string>("market_hash_name", createBuyOrderRequest.MarketHashName),
            new KeyValuePair<string, string>("price_total", createBuyOrderRequest.PriceTotal.ToString()),
            new KeyValuePair<string, string>("quantity", createBuyOrderRequest.Quantity.ToString()),
            new KeyValuePair<string, string>("billing_state", string.Empty),
            new KeyValuePair<string, string>("save_my_address", "0"),
        });
        
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(requestUri),
            Method = HttpMethod.Post,
            Content = content,
        };
        request.Headers.Referrer = new Uri($"https://steamcommunity.com/market/listings/730/{urlEncodedHashName}");
        
        var response = await SteamHttpClient.PostAsync(requestUri, content, cancellationToken);
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
        const string requestUri = "https://steamcommunity.com/market/cancelbuyorder/";
        
        var content = new FormUrlEncodedContent(new []
        {
            new KeyValuePair<string, string>("sessionid", SessionId),
            new KeyValuePair<string, string>("buy_orderid", buyOrderId.ToString()),
        });
        
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(requestUri),
            Method = HttpMethod.Post,
            Content = content,
        };
        
        request.Headers.Referrer = new Uri("https://steamcommunity.com/market/");
        
        var response = await SteamHttpClient.PostAsync(requestUri, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<CancelBuyOrderResponse>(stringResponse);
    }

    /// <summary>
    /// Returns price history of the market item.
    /// </summary>
    /// <param name="appId">Game id</param>
    /// <param name="marketHashName">Market hash name.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    public async Task<PriceHistoryResponse?> GetPriceHistory(long appId, string marketHashName, CancellationToken cancellationToken)
    {
        var requestUri = $"https://steamcommunity.com/market/pricehistory?appid={appId}&market_hash_name={HttpUtility.UrlEncode(marketHashName)}";
        
        var response = await SteamHttpClient.GetAsync(requestUri, cancellationToken);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<PriceHistoryResponse>(stringResponse);
    }
    
    /// <summary>
    /// Request to create sell order.
    /// </summary>
    public async Task<CreateSellOrderResponse?> CreateSellOrder(CreateSellOrderRequest createSellOrderRequest, CancellationToken cancellationToken)
    {
        const string requestUri = "https://steamcommunity.com/market/sellitem/";
        
        var content = new FormUrlEncodedContent(new []
        {
            new KeyValuePair<string, string>("sessionid", SessionId),
            new KeyValuePair<string, string>("appid", createSellOrderRequest.AppId.ToString()),
            new KeyValuePair<string, string>("contextid", createSellOrderRequest.ContextId.ToString()),
            new KeyValuePair<string, string>("assetid", createSellOrderRequest.AssetId.ToString()),
            new KeyValuePair<string, string>("amount", createSellOrderRequest.Quantity.ToString()),
            new KeyValuePair<string, string>("price", createSellOrderRequest.Price.ToString()),
        });
        
        var request = new HttpRequestMessage
        {
            RequestUri = new Uri(requestUri),
            Method = HttpMethod.Post,
            Content = content,
        };
        request.Headers.Referrer = new Uri($"https://steamcommunity.com/id/{SteamHttpClient.SteamId}/inventory/");
        
        var response = await SteamHttpClient.PostAsync(requestUri, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<CreateSellOrderResponse>(stringResponse);
    }
    
    /// <summary>
    /// Request to cancel sell order.
    /// </summary>
    public async Task<bool> CancelSellOrder(long listingId, CancellationToken cancellationToken)
    {
        var requestUri = $"https://steamcommunity.com/market/removelisting/{listingId}";
        
        var content = new FormUrlEncodedContent(new []
        { 
            new KeyValuePair<string, string>("sessionid", SessionId),
        });
        
        var response = await SteamHttpClient.PostAsync(requestUri, content, cancellationToken);
        var readAsStringAsync = await response.Content.ReadAsStringAsync();
        return response.StatusCode == HttpStatusCode.OK;
    }


    public void Dispose()
    {
        SteamHttpClient.Dispose();
    }
}