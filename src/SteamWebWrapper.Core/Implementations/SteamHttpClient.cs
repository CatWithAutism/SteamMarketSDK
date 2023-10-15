using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using SteamWebWrapper.Common.Utils;
using SteamWebWrapper.Contracts.Entities.Authorization;
using SteamWebWrapper.Core.Exceptions;
using SteamWebWrapper.Core.Interfaces;

namespace SteamWebWrapper.Core.Implementations;

public class SteamHttpClient : HttpClient, ISteamHttpClient
{
    public string SteamId { get; private set; }

    public SteamHttpClient(string baseUrl, HttpMessageHandler clientHandler) : base(clientHandler)
    {
        SetupHttpClient(baseUrl);
    }

    #region Private
    
    private void SetupHttpClient(string baseUrl)
    {
        if (baseUrl.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(baseUrl));
        }
        
        BaseAddress = new Uri(baseUrl);
        DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:109.0) Gecko/20100101 Firefox/111.0");
        DefaultRequestHeaders.Add("Accept", "text/javascript, text/html, application/xml, text/xml, */*");
        DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
        DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        DefaultRequestHeaders.Add("Connection", "keep-alive");
    }

    /// <summary>
    /// Fetch RSA Key from steam server for specified username.
    /// </summary>
    /// <param name="username">Steam username</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>This data should be valid about 1 hour.</returns>
    /// <exception cref="GetRsaKeyException">Will throw when cannot get RSA data.</exception>
    private async Task<AuthRsaData> GetAuthRsaData(string username, CancellationToken cancellationToken)
    {
        const string getRsaDataPath ="/login/getrsakey";
        
        var data = new StringContent($"username={username}");
        data.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
        
        var response = await PostAsync(getRsaDataPath, data, cancellationToken);
        var rsaJson = await response.Content.ReadFromJsonAsync<AuthRsaData>(JsonSerializerOptions.Default, cancellationToken);
        
        if (rsaJson is not { Success: true } || rsaJson.PublicKeyExp.IsNullOrEmpty() || rsaJson.PublicKeyMod.IsNullOrEmpty())
        {
            throw new GetRsaKeyException($"We cannot get RSA Key for this account - {username}\n");
        }

        return rsaJson;
    }
    #endregion


    /// <summary>
    /// Executes the login by using the Steam Website.
    /// </summary>
    /// <param name="credentials">Your steam credentials.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A bool containing a value, if the login was successful.</returns>
    public async Task<SteamAuthResult> Authorize(SteamAuthCredentials credentials, CancellationToken cancellationToken)
    {
        const string doLoginUrl = "/login/dologin/";
        
        var authRsaData = await GetAuthRsaData(credentials.Login, cancellationToken);
        if (authRsaData.PublicKeyMod.IsNullOrEmpty() || authRsaData.PublicKeyExp.IsNullOrEmpty())
        {
            throw new SteamAuthorizationException($"We cannot get PublicKeyMod or PublicKeyExp. {Environment.NewLine}" +
                                                  $"{nameof(authRsaData.PublicKeyExp)} {authRsaData.PublicKeyExp}{Environment.NewLine}" +
                                                  $"{nameof(authRsaData.PublicKeyMod)} {authRsaData.PublicKeyMod}{Environment.NewLine}" +
                                                  $"{nameof(authRsaData.Timestamp)} {authRsaData.Timestamp}{Environment.NewLine}" +
                                                  $"Serialized value - {JsonSerializer.Serialize(authRsaData)}{Environment.NewLine}"); 
        }
        
        var encryptedData = CryptoUtils.GetRsaEncryptedPassword(authRsaData.PublicKeyExp!, authRsaData.PublicKeyMod!, credentials.Password);
        var encryptedBase64Password = CryptoUtils.ConvertToBase64String(encryptedData);
        
        var rsaTimestamp = Uri.EscapeDataString(authRsaData.Timestamp!);
        var unixTimeStamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        
        var data = new MultipartFormDataContent
        {
            { new StringContent(encryptedBase64Password), "password" },
            { new StringContent(credentials.Login), "username" },
            { new StringContent(credentials.TwoFactor), "twofactorcode" },
            { new StringContent(credentials.MachineName), "loginfriendlyname" },
            { new StringContent("true"), "remember_login" },
            { new StringContent(unixTimeStamp + "000"), "donotcache" },
            { new StringContent(rsaTimestamp), "rsatimestamp" }
        };
        
        var authResponse = await PostAsync(doLoginUrl, data, cancellationToken);
        authResponse.EnsureSuccessStatusCode();
        
        var authJsonResponse = await authResponse.Content.ReadAsStringAsync(cancellationToken);
        var authResult = JsonSerializer.Deserialize<SteamAuthResult>(authJsonResponse);
        if (authResult == null)
        {
            throw new SteamAuthorizationException($"We cannot deserialize this data - {authJsonResponse}");
        }

        if (authResult is { Success: true, LoginComplete: true })
        {
            SteamId = authResult.TransferParameters.SteamId;
        }
        
        return authResult;
    }
}