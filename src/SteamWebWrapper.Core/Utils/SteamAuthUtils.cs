using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text.Json;
using SteamWebWrapper.Core.Entities.Auth;
using SteamWebWrapper.Core.Exceptions;

namespace SteamWebWrapper.Core.Utils;

public static class SteamAuthUtils
{
    private const string GetRsaDataPath ="https://steamcommunity.com/login/getrsakey";
    
    /// <summary>
    /// Fetch RSA Key from steam server for specified username.
    /// </summary>
    /// <param name="httpClient">Your http client</param>
    /// <param name="username">Steam username</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>This data should be valid about 1 hour.</returns>
    /// <exception cref="GetRsaKeyException">Will throw when cannot get RSA data.</exception>
    public static async Task<AuthRsaData> GetAuthRsaData(HttpClient httpClient, string username, CancellationToken cancellationToken)
    {
        var data = new StringContent($"username={username}");
        data.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        
        data.Headers.TryAddWithoutValidation("Accept", "*/*");
        data.Headers.TryAddWithoutValidation("Referer", @"http:\/\/steamcommunity.com/trade/1");
        data.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36");
        
        var response = await httpClient.PostAsync(GetRsaDataPath, data, cancellationToken);
        var responseStr = await response.Content.ReadAsStringAsync(cancellationToken);
        var rsaJson = await response.Content.ReadFromJsonAsync<AuthRsaData>(JsonSerializerOptions.Default, cancellationToken);
        
        if (rsaJson is not { Success: true } || rsaJson.PublicKeyExp.IsNullOrEmpty() || rsaJson.PublicKeyMod.IsNullOrEmpty())
        {
            throw new GetRsaKeyException($"We cannot get RSA Key for this account - {username}\n");
        }

        return rsaJson;
    }
}