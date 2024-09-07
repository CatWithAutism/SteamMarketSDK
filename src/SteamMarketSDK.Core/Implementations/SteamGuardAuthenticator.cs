using SteamMarketSDK.Common.Utils;
using SteamMarketSDK.Core.Contracts.Entities.SteamGuard;
using SteamMarketSDK.Core.Contracts.Interfaces;
using SteamMarketSDK.SteamGuard;

namespace SteamMarketSDK.Core.Implementations;

public class SteamGuardAuthenticator : SteamGuardAccount, ISteamGuardAuthenticator
{
	private const int AuthenticatorDelay = 20 * 1000;

	public async Task<bool> AcceptDeviceConfirmationAsync()
	{
		var confirmations = await FetchConfirmationsAsync();

		var confirmation = confirmations?.Where(t => t.ConfType == Confirmation.EMobileConfirmationType.MarketListing)
			.ToArray();
		if (confirmation.IsNullOrEmpty())
		{
			return false;
		}

		return await AcceptMultipleConfirmations(confirmation);
	}

	public async Task<string> GetDeviceCodeAsync(bool previousCodeWasIncorrect)
	{
		if (previousCodeWasIncorrect)
		{
			await Task.Delay(AuthenticatorDelay);
		}

		return await GenerateSteamGuardCodeAsync();
	}

	public async Task<string> GetEmailCodeAsync(string email, bool previousCodeWasIncorrect) =>
		throw new NotImplementedException();
}