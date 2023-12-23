using System.Security;
using System.Text.Json.Serialization;

namespace SteamWebWrapper.Core.Contracts.Entities.Authorization;

[SecuritySafeCritical]
public class SteamAuthCredentials
{
    [JsonPropertyName("Login")]
    public string Login { get; set; }
    
    [JsonPropertyName("Password")]
    public string Password { get; set; }
    
    [JsonPropertyName("TwoFactor")]
    public string? TwoFactor { get; set; }

    [JsonPropertyName("MachineName")]
    public string? MachineName { get; set; }
}