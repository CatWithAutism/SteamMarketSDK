using System.Collections.Specialized;
using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SteamWebWrapper.Core.Entities.Auth;
using SteamWebWrapper.Core.Exceptions;
using SteamWebWrapper.Core.Interfaces;
using SteamWebWrapper.Core.Utils;

namespace SteamWebWrapper.Core.Implementations;

public class SteamHttpClient : HttpClient, ISteamHttpClient
{
    private readonly CookieCollection _cookieCollection;
    private HttpClient _httpClient;

    public SteamHttpClient(SteamAuthData steamAuthData)
    {
        _httpClient = new HttpClient();
    }

    public SteamHttpClient(CookieCollection cookieCollection)
    {
        _cookieCollection = cookieCollection;
        var httpClientHandler = new HttpClientHandler();
        httpClientHandler.CookieContainer = new CookieContainer();
        httpClientHandler.CookieContainer.Add(cookieCollection);
        
        _httpClient = new HttpClient();
    }


    /// <summary>
    /// Executes the login by using the Steam Website.
    /// </summary>
    /// <param name="username">Your Steam username.</param>
    /// <param name="password">Your Steam password.</param>
    /// <param name="twoFactor">Two factor code.</param>
    /// <param name="machineName">Friendly machine name to avoid two factor again.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A bool containing a value, if the login was successful.</returns>
    public async Task<SteamAuthResult> Authorize(string username, string password, string twoFactor, string machineName, CancellationToken cancellationToken)
    {
        var authRsaData = await SteamAuthUtils.GetAuthRsaData(_httpClient, username, cancellationToken);
        var encryptedData = CryptoUtils.GetRsaEncryptedPassword(authRsaData, password);
        var encryptedBase64Password = CryptoUtils.ConvertToBase64String(encryptedData);

        var time = Uri.EscapeDataString(authRsaData.Timestamp);
        var unixTimeStamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        
        var data = new MultipartFormDataContent
        {
            { new StringContent(encryptedBase64Password), "password" },
            { new StringContent(username), "username" },
            { new StringContent(twoFactor), "twofactorcode" },
            { new StringContent(machineName), "loginfriendlyname" },
            { new StringContent("true"), "remember_login" },
            { new StringContent(unixTimeStamp + "000"), "donotcache" },
            { new StringContent(time), "rsatimestamp" }
        };

        var authResponse = await _httpClient.PostAsync("https://steamcommunity.com/login/dologin/", data, cancellationToken);
        var textData = await authResponse.Content.ReadAsStringAsync(cancellationToken);
        var authResult = await authResponse.Content.ReadFromJsonAsync<SteamAuthResult>(options:null, cancellationToken);

        return authResult;
    }

    public async Task<bool> GetAuthorizationStatus()
    {
        throw new NotImplementedException();
    }

    
    
}