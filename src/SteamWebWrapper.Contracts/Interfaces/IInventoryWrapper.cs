using SteamWebWrapper.Contracts.Entities.Inventory;

namespace SteamWebWrapper.Contracts.Interfaces;

public interface IInventoryWrapper : IDisposable
{
	Task<InventoryResponse?> GetInventory(string steamId, uint appId, uint assetId, string language, int count);
}