using System.Text.Json.Serialization;

namespace SteamWebWrapper.Core.Contracts.Entities.Authorization;

public class TransferParameters
{
    [JsonPropertyName("steamid")]
    public required string SteamId { get; set; }

    [JsonPropertyName("token_secure")]
    public required string TokenSecure { get; set; }

    [JsonPropertyName("auth")]
    public required string Auth { get; set; }

    [JsonPropertyName("remember_login")]
    public required bool RememberLogin { get; set; }
}