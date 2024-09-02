using SteamWebWrapper.Contracts.Entities.Inventory;
using SteamWebWrapper.Contracts.Interfaces;
using SteamWebWrapper.Core.Contracts.Interfaces;
using SteamWebWrapper.Core.Implementations;
using System.Text.Json;

namespace SteamWebWrapper.Implementations;

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