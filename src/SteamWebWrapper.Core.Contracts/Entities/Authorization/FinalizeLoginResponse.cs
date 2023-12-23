using System.Text.Json.Serialization;

namespace SteamWebWrapper.Core.Contracts.Entities.Authorization;

[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public class FinalizeLoginResponse
{
    [JsonPropertyName("steamID")]
    public long SteamId { get; set; }

    [JsonPropertyName("redir")]
    public string Redir { get; set; }

    [JsonPropertyName("transfer_info")]
    public List<TransferInfo> TransferInfo { get; set; }

    [JsonPropertyName("primary_domain")]
    public string PrimaryDomain { get; set; }
}