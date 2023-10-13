using System.Text.Json.Serialization;

namespace SteamWebWrapper.Core.Entities.Authorization;
public class SteamAuthResult
{
    [JsonPropertyName("success")]
    public required bool Success { get; set; }

    [JsonPropertyName("requires_twofactor")]
    public bool RequiresTwoFactor { get; set; }

    [JsonPropertyName("login_complete")]
    public required bool LoginComplete { get; set; }

    [JsonPropertyName("transfer_urls")]
    public List<string> TransferUrls { get; set; }

    [JsonPropertyName("transfer_parameters")]
    public TransferParameters TransferParameters { get; set; }
}

