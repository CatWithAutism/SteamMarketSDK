using System.Collections.Specialized;
using SteamWebWrapper.Core.Interfaces;
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
        var confirmations = await this.FetchConfirmationsAsync();

        var confirmation = confirmations?.FirstOrDefault(t => t.ConfType == Confirmation.EMobileConfirmationType.FeatureOptOut);
        if (confirmation == null)
        {
            return false;
        }

        return await AcceptConfirmation(confirmation);
    }
    
}