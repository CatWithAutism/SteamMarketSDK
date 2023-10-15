using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Authorization;

public class TransferParameters
{
    [JsonPropertyName("steamid")]
    public string? SteamId { get; set; }

    [JsonPropertyName("token_secure")]
    public string TokenSecure { get; set; }

    [JsonPropertyName("auth")]
    public string Auth { get; set; }

    [JsonPropertyName("remember_login")]
    public bool RememberLogin { get; set; }
}