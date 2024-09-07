using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Privacy;

public class PrivacyResponse
{
	[JsonPropertyName("Privacy")] public required Privacy Privacy { get; set; }

	[JsonPropertyName("success")] public int Success { get; set; }
}