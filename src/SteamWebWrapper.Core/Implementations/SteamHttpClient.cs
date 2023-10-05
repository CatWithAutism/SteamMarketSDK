using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SteamWebWrapper.Core.Entities.Auth;
using SteamWebWrapper.Core.Interfaces;
using SteamWebWrapper.Core.Utils;

namespace SteamWebWrapper.Core.Implementations;

public class SteamHttpClient : HttpClient, ISteamHttpClient
{
    private readonly CookieCollection _cookieCollection;
    private HttpClient _httpClient;

    public SteamHttpClient(SteamAuthData steamAuthData)
    {
        
    }

    public SteamHttpClient(CookieCollection cookieCollection)
    {
        _cookieCollection = cookieCollection;
        var httpClientHandler = new HttpClientHandler();
        httpClientHandler.CookieContainer = new CookieContainer();
        httpClientHandler.CookieContainer.Add(cookieCollection);
        
        _httpClient = new HttpClient();
    }


    /*
    /// <summary>
    /// Executes the login by using the Steam Website.
    /// </summary>
    /// <param name="username">Your Steam username.</param>
    /// <param name="password">Your Steam password.</param>
    /// <returns>A bool containing a value, if the login was successful.</returns>
    private bool DoLogin(string username, string password)
    {
        const string getRsaKeyPath = "https://steamcommunity.com/login/getrsakey";
        // First get the RSA key with which we will encrypt our password.
        var data = new MultipartFormDataContent();
        data.Add(new StringContent("username"), username);
        _httpClient.PostAsync(getRsaKeyPath, )
        string response = Fetch(getRsaKeyPath, "POST", data, false);
        AuthRsaKey? rsaJson = JsonSerializer.Deserialize<AuthRsaKey>(response);

        // Validate, if we could get the rsa key.
        if (!rsaJson.Success)
        {
            return false;
        }

        // RSA Encryption.
        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        RSAParameters rsaParameters = new RSAParameters
        {
            Exponent = CryptoUtils.HexToByteArray(rsaJson.PublicKeyExp),
            Modulus = CryptoUtils.HexToByteArray(rsaJson.PublicKeyMod)
        };

        rsa.ImportParameters(rsaParameters);

        // Encrypt the password and convert it.
        byte[] bytePassword = Encoding.ASCII.GetBytes(password);
        byte[] encodedPassword = rsa.Encrypt(bytePassword, false);
        string encryptedBase64Password = Convert.ToBase64String(encodedPassword);

        SteamResult loginJson = null;
        CookieCollection cookieCollection;
        string steamGuardText = "";
        string steamGuardId = "";

        // Do this while we need a captcha or need email authentification. Probably you have misstyped the captcha or the SteamGaurd code if this comes multiple times.
        do
        {
            Console.WriteLine("SteamWeb: Logging In...");

            bool captcha = loginJson != null && loginJson.captcha_needed;
            bool steamGuard = loginJson != null && loginJson.emailauth_needed;

            string time = Uri.EscapeDataString(rsaJson.timestamp);

            string capGid = string.Empty;
            // Response does not need to send if captcha is needed or not.
            // ReSharper disable once MergeSequentialChecks
            if (loginJson != null && loginJson.captcha_gid != null)
            {
                capGid = Uri.EscapeDataString(loginJson.captcha_gid);
            }

            data = new NameValueCollection { { "password", encryptedBase64Password }, { "username", username } };

            // Captcha Check.
            string capText = "";
            if (captcha)
            {
                Console.WriteLine("SteamWeb: Captcha is needed.");
                System.Diagnostics.Process.Start("https://steamcommunity.com/public/captcha.php?gid=" +
                                                 loginJson.captcha_gid);
                Console.WriteLine("SteamWeb: Type the captcha:");
                string consoleText = Console.ReadLine();
                if (!string.IsNullOrEmpty(consoleText))
                {
                    capText = Uri.EscapeDataString(consoleText);
                }
            }

            data.Add("captchagid", captcha ? capGid : "");
            data.Add("captcha_text", captcha ? capText : "");
            // Captcha end.
            // Added Header for two factor code.
            data.Add("twofactorcode", "");

            // Added Header for remember login. It can also set to true.
            data.Add("remember_login", "false");

            // SteamGuard check. If SteamGuard is enabled you need to enter it. Care probably you need to wait 7 days to trade.
            // For further information about SteamGuard see: https://support.steampowered.com/kb_article.php?ref=4020-ALZM-5519&l=english.
            if (steamGuard)
            {
                Console.WriteLine("SteamWeb: SteamGuard is needed.");
                Console.WriteLine("SteamWeb: Type the code:");
                string consoleText = Console.ReadLine();
                if (!string.IsNullOrEmpty(consoleText))
                {
                    steamGuardText = Uri.EscapeDataString(consoleText);
                }

                steamGuardId = loginJson.emailsteamid;

                // Adding the machine name to the NameValueCollection, because it is requested by steam.
                Console.WriteLine("SteamWeb: Type your machine name:");
                consoleText = Console.ReadLine();
                var machineName = string.IsNullOrEmpty(consoleText) ? "" : Uri.EscapeDataString(consoleText);
                data.Add("loginfriendlyname", machineName != "" ? machineName : "defaultSteamBotMachine");
            }

            data.Add("emailauth", steamGuardText);
            data.Add("emailsteamid", steamGuardId);
            // SteamGuard end.

            // Added unixTimestamp. It is included in the request normally.
            var unixTimestamp = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            // Added three "0"'s because Steam has a weird unix timestamp interpretation.
            data.Add("donotcache", unixTimestamp + "000");

            data.Add("rsatimestamp", time);

            // Sending the actual login.
            using (HttpWebResponse webResponse =
                   Request("https://steamcommunity.com/login/dologin/", "POST", data, false))
            {
                var stream = webResponse.GetResponseStream();
                if (stream == null)
                {
                    return false;
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string json = reader.ReadToEnd();
                    loginJson = JsonConvert.DeserializeObject<SteamResult>(json);
                    cookieCollection = webResponse.Cookies;
                }
            }
        } while (loginJson.captcha_needed || loginJson.emailauth_needed);

        // If the login was successful, we need to enter the cookies to steam.
        if (loginJson.success)
        {
            _cookies = new CookieContainer();
            foreach (Cookie cookie in cookieCollection)
            {
                _cookies.Add(cookie);
            }

            SubmitCookies(_cookies);
            return true;
        }
        else
        {
            Console.WriteLine("SteamWeb Error: " + loginJson.message);
            return false;
        }

    }
*/

    public async Task<bool> GetAuthorizationStatus()
    {
        throw new NotImplementedException();
    }

    
    
}