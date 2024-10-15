using SteamKit2;
using SteamKit2.Authentication;
using SteamKit2.Internal;
using SteamMarketSDK.Common.Utils;
using SteamMarketSDK.Core.Contracts.Constants;
using SteamMarketSDK.Core.Contracts.Entities.Authorization;
using SteamMarketSDK.Core.Contracts.Entities.Exceptions;
using SteamMarketSDK.Core.Contracts.Interfaces;
using SteamMarketSDK.Implementations;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SteamMarketSDK.Core.Implementations;

public class SteamHttpClient : HttpClient
{
	public SteamHttpClient(HttpClientHandler httpClientHandler) : base(httpClientHandler)
	{
		HttpClientHandler = httpClientHandler;
		DefaultRequestHeaders.Add("User-Agent",
			"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.15; rv:109.0) Gecko/20100101 Firefox/111.0");
		DefaultRequestHeaders.Add("Accept", "text/javascript, text/html, application/xml, text/xml, */*");
		DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
		DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
		DefaultRequestHeaders.Add("Origin", SteamEndpoints.CommunityBaseUrl);
		DefaultRequestHeaders.Referrer = new Uri(SteamEndpoints.SteamMarketUrl);
	}

	public virtual string? AccessToken { get; protected set; }
	public virtual string? RefreshToken { get; protected set; }
	public virtual SteamID? SteamId { get; protected set; }

	public virtual string SessionId
	{
		get
		{
			return GetCookie(SteamCommunityUri, "sessionid");
		}
	}

	public virtual string SecureLogin
	{
		get
		{
			return GetCookie(SteamCommunityUri, "steamLoginSecure");
		}
	}

	public virtual string SteamCountry
	{
		get
		{
			return GetCookie(SteamCommunityUri, "steamCountry");
		}
	}

	private ISteamConverter Converter = new SteamConverter();
	private Uri SteamCommunityUri { get; } = new Uri(SteamEndpoints.CommunityBaseUrl);
	private HttpClientHandler HttpClientHandler { get; init; }
	private ISteamGuardAuthenticator? SteamGuardAuthenticator { get; set; }

	public virtual async Task AuthorizeViaOAuthAsync(SteamAuthCredentials credentials,
		ISteamGuardAuthenticator? steamGuardAuthenticator, CancellationToken cancellationToken)
	{
		if (steamGuardAuthenticator != null)
		{
			SteamGuardAuthenticator = steamGuardAuthenticator;
		}

		var steamClient = new SteamClient();
		steamClient.Connect();

		//fuck that
		while (!steamClient.IsConnected)
		{
			await Task.Delay(500);
		}

		var authSession = await steamClient.Authentication.BeginAuthSessionViaCredentialsAsync(new AuthSessionDetails
		{
			Username = credentials.Login,
			Password = credentials.Password,
			Authenticator = steamGuardAuthenticator,
			PlatformType = EAuthTokenPlatformType.k_EAuthTokenPlatformType_WebBrowser,
			IsPersistentSession = true,
			DeviceFriendlyName = credentials.MachineName,
			ClientOSType = EOSType.Macos1014
		});

		var pollResponse = await authSession.PollingWaitForResultAsync(cancellationToken);

		AccessToken = pollResponse.AccessToken;
		RefreshToken = pollResponse.RefreshToken;
		SteamId = authSession.SteamID;

		await EndAuthSession(cancellationToken);
	}

	public virtual async Task<T> GetObjectAsync<T>(string requestUri, CancellationToken cancellationToken)
		where T : notnull
	{
		var stringResponse = await GetStringAsync(requestUri, cancellationToken);
		return Converter.DeserializeObject<T>(stringResponse);
	}

	public virtual async Task<T> GetObjectAsync<T>(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
		where T : notnull
	{
		var response = await SendAsync(requestMessage, cancellationToken);
		response.EnsureSuccessStatusCode();

		var stringContent = await response.Content.ReadAsStringAsync(cancellationToken);
		return Converter.DeserializeObject<T>(stringContent);
	}

	#region Private

	/// <summary>
	///     Fetch RSA Key from steam server for specified username.
	/// </summary>
	/// <param name="username">Steam username</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>This data should be valid about 1 hour.</returns>
	/// <exception cref="GetRsaKeyException">Will throw when cannot get RSA data.</exception>
	private async Task<RsaDataResponse> GetAuthRsaData(string username, CancellationToken cancellationToken)
	{
		const string getRsaDataPath = "/login/getrsakey";

		var data = new StringContent($"username={username}");
		data.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

		var response = await PostAsync(getRsaDataPath, data, cancellationToken);
		var rsaJson =
			await response.Content.ReadFromJsonAsync<RsaDataResponse>(JsonSerializerOptions.Default, cancellationToken);

		if (rsaJson is not { Success: true } || rsaJson.PublicKeyExp.IsNullOrEmpty() ||
		    rsaJson.PublicKeyMod.IsNullOrEmpty())
		{
			throw new GetRsaKeyException($"We cannot get RSA Key for this account - {username}\n");
		}

		return rsaJson;
	}

	private async Task EndAuthSession(CancellationToken cancellationToken)
	{
		const string finalizeAuthPath = "https://login.steampowered.com/jwt/finalizelogin";

		var sessionId = CryptoUtils.GetRandomHexNumber(12);

		var postData = new MultipartFormDataContent();
		postData.Add(
			new StringContent(RefreshToken ??
			                  throw new InvalidOperationException(
				                  $"We cannot get cookies when {RefreshToken} is null")), "nonce");
		postData.Add(new StringContent(sessionId), "sessionid");
		postData.Add(new StringContent("https://steamcommunity.com/login/home/?goto="), "redir");

		var response = await PostAsync(finalizeAuthPath, postData, cancellationToken);
		response.EnsureSuccessStatusCode();

		var responseData = await response.Content.ReadAsStringAsync(cancellationToken);
		var finalizeResponse = JsonSerializer.Deserialize<FinalizeLoginResponse>(responseData) ??
		                       throw new InvalidDataException($"We cannot deserialize response from {responseData}");
		foreach (var info in finalizeResponse.TransferInfo)
		{
			postData = new MultipartFormDataContent();
			postData.Add(new StringContent(info.Params.Nonce), "nonce");
			postData.Add(new StringContent(info.Params.Auth), "auth");
			postData.Add(new StringContent(finalizeResponse.SteamId.ToString()), "steamID");

			response = await PostAsync(info.Url, postData, cancellationToken);
			response.EnsureSuccessStatusCode();
		}
	}

	private string GetCookie(Uri url, string name)
	{
		var cookies = HttpClientHandler.CookieContainer.GetCookies(url);
		return cookies[name]?.Value ?? string.Empty;
	}

	#endregion
}