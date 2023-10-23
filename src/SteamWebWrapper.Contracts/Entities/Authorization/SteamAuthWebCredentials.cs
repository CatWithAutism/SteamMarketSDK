using System.Security;

namespace SteamWebWrapper.Contracts.Entities.Authorization;

[SecuritySafeCritical]
public class SteamAuthWebCredentials
{
    public required string Login { get; set; }
    
    public required string Password { get; set; }

    public required string MachineName { get; set; }
    
    public required string TwoFactor { get; set; }
}