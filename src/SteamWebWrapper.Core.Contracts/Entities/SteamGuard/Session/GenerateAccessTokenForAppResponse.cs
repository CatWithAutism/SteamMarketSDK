using System.Text.Json.Serialization;

namespace SteamWebWrapper.Core.Contracts.Entities.SteamGuard.Session;

public class GenerateAccessTokenForAppResponse
{
	[JsonPropertyName("response")] public GenerateAccessTokenForAppResponseResponse Response;
}