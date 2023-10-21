using System.Collections.Specialized;
using SteamAuth;
using SteamKit2.Authentication;

namespace SteamWebWrapper.Core.Interfaces;

public interface ISteamGuardAuthenticator : IAuthenticator
{
    string SharedSecret { get; set; }
    string SerialNumber { get; set; }
    string RevocationCode { get; set; }
    string URI { get; set; }
    long ServerTime { get; set; }
    string AccountName { get; set; }
    string TokenGID { get; set; }
    string IdentitySecret { get; set; }
    string Secret1 { get; set; }
    int Status { get; set; }
    string DeviceID { get; set; }

    /// <summary>
    /// Set to true if the authenticator has actually been applied to the account.
    /// </summary>
    bool FullyEnrolled { get; set; }

    SessionData Session { get; set; }

    /// <summary>
    /// Remove steam guard from this account
    /// </summary>
    /// <param name="scheme">1 = Return to email codes, 2 = Remove completley</param>
    /// <returns></returns>
    Task<bool> DeactivateAuthenticator(int scheme = 1);

    string GenerateSteamGuardCode();
    Task<string> GenerateSteamGuardCodeAsync();
    string GenerateSteamGuardCodeForTime(long time);
    Confirmation[] FetchConfirmations();
    Task<Confirmation[]> FetchConfirmationsAsync();

    Task<bool> AcceptMultipleConfirmations(Confirmation[] confs);
    Task<bool> DenyMultipleConfirmations(Confirmation[] confs);
    Task<bool> AcceptConfirmation(Confirmation conf);
    Task<bool> DenyConfirmation(Confirmation conf);
    string GenerateConfirmationURL(string tag = "conf");
    string GenerateConfirmationQueryParams(string tag);
    NameValueCollection GenerateConfirmationQueryParamsAsNVC(string tag);
}