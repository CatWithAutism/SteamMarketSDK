using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Authorization;
public class SteamAuthWebResponse
{
    [JsonPropertyName("success")]
    public required bool Success { get; set; }

    [JsonPropertyName("requires_twofactor")]
    public required bool RequiresTwoFactor { get; set; }

    [JsonPropertyName("login_complete")]
    public bool? LoginComplete { get; set; }

    [JsonPropertyName("transfer_urls")]
    public List<string>? TransferUrls { get; set; }

    [JsonPropertyName("transfer_parameters")]
    public TransferParameters? TransferParameters { get; set; }
}

