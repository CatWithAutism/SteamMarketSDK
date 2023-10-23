using System.Security;

namespace SteamWebWrapper.Contracts.Entities.Authorization;

[SecuritySafeCritical]
public class SteamAuthCredentials
{
    public required string Login { get; set; }
    
    public required string Password { get; set; }

    public required string MachineName { get; set; }
}