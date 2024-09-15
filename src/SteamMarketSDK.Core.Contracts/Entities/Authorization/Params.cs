using System.Text.Json.Serialization;

namespace SteamMarketSDK.Core.Contracts.Entities.Authorization;

public class Params
{
	[JsonPropertyName("auth")] public string Auth { get; set; }

	[JsonPropertyName("nonce")] public string Nonce { get; set; }
}