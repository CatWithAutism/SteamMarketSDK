using SteamMarketSDK.Contracts.Entities.Inventory;
using SteamMarketSDK.Contracts.Interfaces;
using SteamMarketSDK.Core.Contracts.Interfaces;
using SteamMarketSDK.Core.Implementations;
using System.Text.Json;

namespace SteamMarketSDK.Implementations;

public class InventoryWrapper : IInventoryWrapper
{
	private readonly SteamHttpClient _steamHttpClient;
	
	public InventoryWrapper(SteamHttpClient steamHttpClient) => _steamHttpClient = steamHttpClient;

	public void Dispose() => _steamHttpClient.Dispose();

	public async Task<InventoryResponse?> GetInventory(string steamId, uint appId, uint assetId, string language,
		int count)
	{
		var requestUri =
			$"https://steamcommunity.com/inventory/{steamId}/{appId}/{assetId}?l={language}&count={count}&norender=true";

		var response = await _steamHttpClient.GetAsync(requestUri);
		response.EnsureSuccessStatusCode();

		var stringResponse = await response.Content.ReadAsStringAsync();
		return JsonSerializer.Deserialize<InventoryResponse>(stringResponse);
	}
}