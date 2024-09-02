using SteamKit2.Authentication;
using SteamWebWrapper.Core.Contracts.Entities.SteamGuard;
using System.Collections.Specialized;

namespace SteamWebWrapper.Core.Contracts.Interfaces;

public interface ISteamGuardAuthenticator : IAuthenticator
{
	string AccountName { get; set; }
	string DeviceId { get; set; }

	/// <summary>
	///     Set to true if the authenticator has actually been applied to the account.
	/// </summary>
	bool FullyEnrolled { get; set; }

	string IdentitySecret { get; set; }
	string RevocationCode { get; set; }
	string Secret1 { get; set; }
	string SerialNumber { get; set; }
	long ServerTime { get; set; }
	string SharedSecret { get; set; }
	int Status { get; set; }
	string TokenGid { get; set; }
	string Uri { get; set; }
	Task<bool> AcceptConfirmation(Confirmation conf);

	Task<bool> AcceptMultipleConfirmations(Confirmation[] confs);

	/// <summary>
	///     Remove steam guard from this account
	/// </summary>
	/// <param name="scheme">1 = Return to email codes, 2 = Remove completley</param>
	/// <returns></returns>
	Task<bool> DeactivateAuthenticator(int scheme = 1);

	Task<bool> DenyConfirmation(Confirmation conf);
	Task<bool> DenyMultipleConfirmations(Confirmation[] confs);
	Confirmation[] FetchConfirmations();
	Task<Confirmation[]> FetchConfirmationsAsync();
	string GenerateConfirmationQueryParams(string tag);
	NameValueCollection GenerateConfirmationQueryParamsAsNvc(string tag);
	string GenerateConfirmationUrl(string tag = "conf");

	string GenerateSteamGuardCode();
	Task<string> GenerateSteamGuardCodeAsync();
	string GenerateSteamGuardCodeForTime(long time);
}