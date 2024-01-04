using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SteamWebWrapper.Core.Contracts.Entities.SteamGuard.Session;
using SteamWebWrapper.SteamGuard.Utils;

namespace SteamWebWrapper.SteamGuard.Extensions;

public static class SessionDataExtension
{
        public static async Task RefreshAccessToken(this SessionData sessionData)
        {
            if (string.IsNullOrEmpty(sessionData.RefreshToken))
                throw new Exception("Refresh token is empty");

            if (IsTokenExpired(sessionData.RefreshToken))
                throw new Exception("Refresh token is expired");

            string responseStr;
            try
            {
                var postData = new NameValueCollection
                {
                    { "refresh_token", sessionData.RefreshToken },
                    { "steamid", sessionData.SteamId.ToString() }
                };
                responseStr = await SteamWeb.PostRequest("https://api.steampowered.com/IAuthenticationService/GenerateAccessTokenForApp/v1/", null, postData);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to refresh token: " + ex.Message);
            }

            var response = JsonSerializer.Deserialize<GenerateAccessTokenForAppResponse>(responseStr);
            sessionData.AccessToken = response.Response.AccessToken;
        }
        
        public static bool IsAccessTokenExpired(this SessionData sessionData)
        {
            return string.IsNullOrEmpty(sessionData.AccessToken) || IsTokenExpired(sessionData.AccessToken);
        }

        public static bool IsRefreshTokenExpired(this SessionData sessionData)
        {
            return string.IsNullOrEmpty(sessionData.RefreshToken) || IsTokenExpired(sessionData.RefreshToken);
        }

        private static bool IsTokenExpired(string token)
        {
            var tokenComponents = token.Split('.');
            // Fix up base64url to normal base64
            var base64 = tokenComponents[1].Replace('-', '+').Replace('_', '/');

            if (base64.Length % 4 != 0)
            {
                base64 += new string('=', 4 - base64.Length % 4);
            }

            var payloadBytes = Convert.FromBase64String(base64);
            var jwt = JsonSerializer.Deserialize<SteamAccessToken>(Encoding.UTF8.GetString(payloadBytes));

            // Compare expire time of the token to the current time
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds() > jwt.Exp;
        }

        public static CookieContainer GetCookies(this SessionData sessionData)
        {
            sessionData.SessionId ??= GenerateSessionId();

            var cookies = new CookieContainer();
            foreach (var domain in new[] { "steamcommunity.com", "store.steampowered.com" })
            {
                cookies.Add(new Cookie("steamLoginSecure", sessionData.GetSteamLoginSecure(), "/", domain));
                cookies.Add(new Cookie("sessionid", sessionData.SessionId, "/", domain));
                cookies.Add(new Cookie("mobileClient", "android", "/", domain));
                cookies.Add(new Cookie("mobileClientVersion", "777777 3.6.4", "/", domain));
            }
            return cookies;
        }

        private static string GetSteamLoginSecure(this SessionData sessionData)
        {
            return sessionData.SteamId + "%7C%7C" + sessionData.AccessToken;
        }

        private static string GenerateSessionId()
        {
            return MathUtils.GetRandomHexNumber(32);
        }
}