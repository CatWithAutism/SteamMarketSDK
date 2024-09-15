using System.Text.Json.Serialization;

namespace SteamMarketSDK.Core.Contracts.Entities.SteamGuard.Session;

public class GenerateAccessTokenForAppResponse
{
	[JsonPropertyName("response")] public GenerateAccessTokenForAppResponseResponse Response;
}