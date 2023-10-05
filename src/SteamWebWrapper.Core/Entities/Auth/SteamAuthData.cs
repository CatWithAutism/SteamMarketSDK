using System.Security;

namespace SteamWebWrapper.Core.Entities.Auth;

[SecuritySafeCritical]
public class SteamAuthData
{
    public required SecureString Login { get; set; }
    
    public required SecureString Password { get; set; }

    public SecureString? TwoFactor { get; set; }
}