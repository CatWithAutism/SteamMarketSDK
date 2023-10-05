using System.Text.Json.Serialization;

namespace SteamWebWrapper.Core.Entities.Account.Privacy;

public class PrivacyResponse
{
    [JsonPropertyName("success")]
    public int Success { get; set; }

    [JsonPropertyName("Privacy")]
    public required Privacy Privacy { get; set; }
}