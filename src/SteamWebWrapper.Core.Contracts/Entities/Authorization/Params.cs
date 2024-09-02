using System.Text.Json.Serialization;

namespace SteamWebWrapper.Core.Contracts.Entities.Authorization;

public class Params
{
	[JsonPropertyName("auth")] public string Auth { get; set; }

	[JsonPropertyName("nonce")] public string Nonce { get; set; }
}