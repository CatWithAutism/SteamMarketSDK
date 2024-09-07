using SteamMarketSDK.Contracts.Entities.Inventory;

namespace SteamMarketSDK.Contracts.Interfaces;

public interface IInventoryWrapper : IDisposable
{
	Task<InventoryResponse?> GetInventory(string steamId, uint appId, uint assetId, string language, int count);
}