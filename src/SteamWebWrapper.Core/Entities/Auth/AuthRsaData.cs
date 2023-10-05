using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace SteamWebWrapper.Core.Entities.Auth;

public class AuthRsaData
{
    [JsonPropertyName("success")]
    public required bool Success { get; set; }

    [JsonPropertyName("publickey_mod")]
    public string? PublicKeyMod { get; set; }

    [JsonPropertyName("publickey_exp")]
    public string? PublicKeyExp { get; set; }

    [JsonPropertyName("timestamp")]
    public string? Timestamp { get; set; }
}