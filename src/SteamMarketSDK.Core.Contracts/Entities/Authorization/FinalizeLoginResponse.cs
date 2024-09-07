using System.Text.Json.Serialization;

namespace SteamMarketSDK.Core.Contracts.Entities.Authorization;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class FinalizeLoginResponse
{
	[JsonPropertyName("primary_domain")] public string PrimaryDomain { get; set; }

	[JsonPropertyName("redir")] public string Redir { get; set; }

	[JsonPropertyName("steamID")] public long SteamId { get; set; }

	[JsonPropertyName("transfer_info")] public List<TransferInfo> TransferInfo { get; set; }
}