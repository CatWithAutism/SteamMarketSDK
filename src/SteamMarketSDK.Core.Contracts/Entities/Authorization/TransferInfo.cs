using System.Text.Json.Serialization;

namespace SteamMarketSDK.Core.Contracts.Entities.Authorization;

public class TransferInfo
{
	[JsonPropertyName("params")] public Params Params { get; set; }

	[JsonPropertyName("url")] public string Url { get; set; }
}