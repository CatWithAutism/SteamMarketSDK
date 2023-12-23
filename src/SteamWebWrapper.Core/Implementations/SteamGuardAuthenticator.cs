using SteamKit2.Internal;
using SteamWebWrapper.Common.Utils;
using SteamWebWrapper.Core.Contracts.Interfaces;
using SteamWebWrapper.SteamGuard;

namespace SteamWebWrapper.Core.Implementations;

public class SteamGuardAuthenticator : SteamGuardAccount, ISteamGuardAuthenticator
{
    private const int AuthenticatorDelay = 20 * 1000;
    
    public async Task<string> GetDeviceCodeAsync(bool previousCodeWasIncorrect)
    {
        if (previousCodeWasIncorrect)
        {
            await Task.Delay(AuthenticatorDelay);
        }

        return await GenerateSteamGuardCodeAsync();
    }

    public async Task<string> GetEmailCodeAsync(string email, bool previousCodeWasIncorrect)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AcceptDeviceConfirmationAsync()
    {
        var confirmations = await FetchConfirmationsAsync();

        var confirmation = confirmations?.Where(t => t.ConfType == Confirmation.EMobileConfirmationType.MarketListing).ToArray();
        if (confirmation.IsNullOrEmpty())
        {
            return false;
        }

        return await AcceptMultipleConfirmations(confirmation);
    }
}