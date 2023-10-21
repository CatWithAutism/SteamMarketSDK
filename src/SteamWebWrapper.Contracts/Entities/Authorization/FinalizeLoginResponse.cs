using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Authorization;

public class FinalizeLoginResponse
{
    [JsonPropertyName("steamID")]
    public string SteamID { get; set; }

    [JsonPropertyName("redir")]
    public string Redir { get; set; }

    [JsonPropertyName("transfer_info")]
    public List<TransferInfo> TransferInfo { get; set; }

    [JsonPropertyName("primary_domain")]
    public string PrimaryDomain { get; set; }
}