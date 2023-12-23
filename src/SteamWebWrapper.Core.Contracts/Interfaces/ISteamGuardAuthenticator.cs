using System.Collections.Specialized;
using SteamKit2.Authentication;
using SteamKit2.Internal;
using SteamWebWrapper.Core.Contracts.Entities.SteamGuard;

namespace SteamWebWrapper.Core.Contracts.Interfaces;

public interface ISteamGuardAuthenticator : IAuthenticator
{
    string SharedSecret { get; set; }
    string SerialNumber { get; set; }
    string RevocationCode { get; set; }
    string Uri { get; set; }
    long ServerTime { get; set; }
    string AccountName { get; set; }
    string TokenGid { get; set; }
    string IdentitySecret { get; set; }
    string Secret1 { get; set; }
    int Status { get; set; }
    string DeviceId { get; set; }

    /// <summary>
    /// Set to true if the authenticator has actually been applied to the account.
    /// </summary>
    bool FullyEnrolled { get; set; }

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
    string GenerateConfirmationUrl(string tag = "conf");
    string GenerateConfirmationQueryParams(string tag);
    NameValueCollection GenerateConfirmationQueryParamsAsNvc(string tag);
}