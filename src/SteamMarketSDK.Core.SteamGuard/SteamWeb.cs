using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SteamMarketSDK.SteamGuard;

public class SteamWeb
{
	public const string MobileAppUserAgent = "Dalvik/2.1.0 (Linux; U; Android 9; Valve Steam App Version/3)";

	public static async Task<string> GetRequest(string url, CookieContainer cookies)
	{
		using var webClient = new CookieAwareWebClient();
		webClient.Encoding = Encoding.UTF8;
		webClient.CookieContainer = cookies;
		webClient.Headers[HttpRequestHeader.UserAgent] = MobileAppUserAgent;

		var response = await webClient.DownloadStringTaskAsync(url);
		return response;
	}

	public static async Task<string> PostRequest(string url, CookieContainer cookies, NameValueCollection body)
	{
		body ??= [];

		using var webClient = new CookieAwareWebClient();
		webClient.Encoding = Encoding.UTF8;
		webClient.CookieContainer = cookies;
		webClient.Headers[HttpRequestHeader.UserAgent] = MobileAppUserAgent;
		var result = await webClient.UploadValuesTaskAsync(new Uri(url), "POST", body);

		var response = Encoding.UTF8.GetString(result);
		return response;
	}
}