using System.Text.Json.Serialization;

namespace SteamWebWrapper.Core.Entities.Auth;

public class SteamAuthResult
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("requires_twofactor")]
    public bool RequiresTwoFactor { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}