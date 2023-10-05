using System.Security;

namespace SteamWebWrapper.Core.Entities.Auth;

[SecuritySafeCritical]
public class SteamAuthData
{
    public required string Login { get; set; }
    
    public required string Password { get; set; }

    public string? TwoFactor { get; set; }
}