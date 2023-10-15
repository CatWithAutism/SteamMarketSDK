using System.Net.Http.Json;
using System.Text.Json;
using SteamWebWrapper.Contracts.Entities.Market.History;
using SteamWebWrapper.Core.Interfaces;
using SteamWebWrapper.Interfaces;

namespace SteamWebWrapper.Implementations;

public class InventoryWrapper
{
    private readonly ISteamHttpClient _steamHttpClient;

    public InventoryWrapper(ISteamHttpClient steamHttpClient)
    {
        _steamHttpClient = steamHttpClient;
    }
    
    public async Task<MarketHistoryResponse?> GetInventory(long offset, long count)
    {
        var requestUri = $"/market/myhistory/?query=&count={count}&start={offset}&norender=true";
        
        var response = await _steamHttpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MarketHistoryResponse>(stringResponse);
    }

    public void Dispose()
    {
        _steamHttpClient.Dispose();
    }
}