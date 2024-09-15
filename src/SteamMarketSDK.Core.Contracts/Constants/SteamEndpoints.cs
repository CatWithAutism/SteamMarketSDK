namespace SteamMarketSDK.Core.Contracts.Constants;

public static class SteamEndpoints
{
	public const string CommunityBaseUrl = "https://steamcommunity.com";
	public const string SteamMarketUrl = CommunityBaseUrl + "/market";
	public const string MobileAuthBaseUrl = SteamApiBaseUrl + "/IMobileAuthService/%s/v0001";
	public const string SteamApiBaseUrl = "https://api.steampowered.com";
	public const string TwoFactorBaseUrl = SteamApiBaseUrl + "/ITwoFactorService/%s/v0001";
	public static string MobileAuthGetTokenUrl = MobileAuthBaseUrl.Replace("%s", "GetWGToken");
	public static string TwoFactorTimeQueryUrl = TwoFactorBaseUrl.Replace("%s", "QueryTime");
}