namespace SteamMarketSDK.Core.Contracts.Entities.SteamGuard.Session;

public class SessionData
{
	public string? AccessToken { get; set; }

	public string? RefreshToken { get; set; }

	public string? SessionId { get; set; }
	public ulong SteamId { get; set; }
}