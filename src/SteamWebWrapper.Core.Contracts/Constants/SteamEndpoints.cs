namespace SteamWebWrapper.Core.Contracts.Constants;

public static class SteamEndpoints
{
	public const string CommunityBase = "https://steamcommunity.com";
	public const string MobileAuthBase = SteamApiBase + "/IMobileAuthService/%s/v0001";
	public const string SteamApiBase = "https://api.steampowered.com";
	public const string TwoFactorBase = SteamApiBase + "/ITwoFactorService/%s/v0001";
	public static string MobileAuthGetToken = MobileAuthBase.Replace("%s", "GetWGToken");
	public static string TwoFactorTimeQuery = TwoFactorBase.Replace("%s", "QueryTime");
}