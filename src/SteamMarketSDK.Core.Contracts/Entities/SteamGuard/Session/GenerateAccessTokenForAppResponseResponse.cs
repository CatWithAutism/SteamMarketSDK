using System.Text.Json.Serialization;

namespace SteamMarketSDK.Core.Contracts.Entities.SteamGuard.Session;

public class GenerateAccessTokenForAppResponseResponse
{
	[JsonPropertyName("access_token")] public string AccessToken { get; set; }
}