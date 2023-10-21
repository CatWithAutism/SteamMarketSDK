using System.Text.Json;
using System.Text.RegularExpressions;
using SteamWebWrapper.Contracts.Entities.Account;
using SteamWebWrapper.Contracts.Entities.Market.CreateBuyOrder;
using SteamWebWrapper.Contracts.Entities.Market.History;
using SteamWebWrapper.Core.Interfaces;
using SteamWebWrapper.Interfaces;

namespace SteamWebWrapper.Implementations;

public class MarketWrapper : IMarketWrapper
{
    private readonly ISteamHttpClient _steamHttpClient;

    public MarketWrapper(ISteamHttpClient steamHttpClient)
    {
        _steamHttpClient = steamHttpClient;
    }
    
    public async Task<MarketHistoryResponse?> GetMarketHistoryAsync(long offset, long count)
    {
        var requestUri = $"https://steamcommunity.com/market/myhistory/?query=&count={count}&start={offset}&norender=true";
        
        var response = await _steamHttpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MarketHistoryResponse>(stringResponse);
    }
    
    public async Task<AccountInfo?> GetMarketInfoAsync(CancellationToken cancellationToken)
    {
        const string infoPage = "https://steamcommunity.com/market/#";
        
        var response = await _steamHttpClient.GetAsync(infoPage, cancellationToken);
        response.EnsureSuccessStatusCode();

        var webPage = await response.Content.ReadAsStringAsync(cancellationToken);
        var match = Regex.Match(webPage, @"{\s*\""wallet_currency\""[A-Za-z0-9:\.\s,\""\\_\-]+}");

        return JsonSerializer.Deserialize<AccountInfo>(match.Value);
    }
    
    private async Task<CreateBuyOrderResponse> CreateBuyOrderAsync(CreateBuyOrderRequest request)
    {
        throw new NotImplementedException();
        const string createBuyOrderPath ="/market/createbuyorder/";

        var data = new StringContent($"sessionid={""}&" +
                                     $"currency={request.Currency}&" +
                                     $"appid={request.AppId}&" +
                                     $"market_hash_name={request.MarketHashName}&" +
                                     $"price_total={request.TotalPrice}&" +
                                     $"quantity={request.Quantity}&" +
                                     $"billing_state=0&" +
                                     $"save_my_address=0");
        
        /*var data = new StringContent($"username={username}");
        data.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        
        var response = await PostAsync(getRsaDataPath, data, cancellationToken);
        var rsaJson = await response.Content.ReadFromJsonAsync<RsaDataResponse>(JsonSerializerOptions.Default, cancellationToken);
        
        if (rsaJson is not { Success: true } || rsaJson.PublicKeyExp.IsNullOrEmpty() || rsaJson.PublicKeyMod.IsNullOrEmpty())
        {
            throw new GetRsaKeyException($"We cannot get RSA Key for this account - {username}\n");
        }

        return rsaJson;*/

        return null;
    }

    public void Dispose()
    {
        _steamHttpClient.Dispose();
    }
}