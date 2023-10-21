using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Authorization;

public class Params
{
    [JsonPropertyName("nonce")]
    public string Nonce { get; set; }

    [JsonPropertyName("auth")]
    public string Auth { get; set; }
}