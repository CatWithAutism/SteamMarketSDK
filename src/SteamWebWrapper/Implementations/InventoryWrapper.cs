using System.Net.Http.Json;
using System.Text.Json;
using SteamWebWrapper.Contracts.Entities.Inventory;
using SteamWebWrapper.Contracts.Entities.Market.History;
using SteamWebWrapper.Core.Interfaces;
using SteamWebWrapper.Interfaces;

namespace SteamWebWrapper.Implementations;

public class InventoryWrapper : IInventoryWrapper
{
    private readonly ISteamHttpClient _steamHttpClient;

    public InventoryWrapper(ISteamHttpClient steamHttpClient)
    {
        _steamHttpClient = steamHttpClient;
    }
    
    public async Task<InventoryResponse?> GetInventory(string steamId, uint appId, uint assetId, string language, int count)
    {
        var requestUri = $"/inventory/{steamId}/{appId}/{assetId}?l={language}&count={count}&norender=true";
        
        var response = await _steamHttpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<InventoryResponse>(stringResponse);
    }

    public void Dispose()
    {
        _steamHttpClient.Dispose();
    }
}