using Newtonsoft.Json;
using SteamMarketSDK.Core.Contracts.Constants;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SteamMarketSDK.SteamGuard;

/// <summary>
///     Class to help align system time with the Steam server time. Not super advanced; probably not taking some things
///     into account that it should.
///     Necessary to generate up-to-date codes. In general, this will have an error of less than a second, assuming Steam
///     is operational.
/// </summary>
public class TimeAligner
{
	private static bool _aligned;
	private static int _timeDifference;

	public static void AlignTime()
	{
		var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		using (var client = new WebClient())
		{
			client.Encoding = Encoding.UTF8;
			try
			{
				var response = client.UploadString(SteamEndpoints.TwoFactorTimeQueryUrl, "steamid=0");
				var query = JsonConvert.DeserializeObject<TimeQuery>(response);
				_timeDifference = (int) (query.Response.ServerTime - currentTime);
				_aligned = true;
			}
			catch (WebException)
			{
			}
		}
	}

	public static async Task AlignTimeAsync()
	{
		var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		var client = new WebClient();
		try
		{
			client.Encoding = Encoding.UTF8;
			var response = await client.UploadStringTaskAsync(new Uri(SteamEndpoints.TwoFactorTimeQueryUrl), "steamid=0");
			var query = JsonConvert.DeserializeObject<TimeQuery>(response);
			_timeDifference = (int) (query.Response.ServerTime - currentTime);
			_aligned = true;
		}
		catch (WebException)
		{
		}
	}

	public static long GetSteamTime()
	{
		if (!_aligned)
		{
			AlignTime();
		}

		return DateTimeOffset.UtcNow.ToUnixTimeSeconds() + _timeDifference;
	}

	public static async Task<long> GetSteamTimeAsync()
	{
		if (!_aligned)
		{
			await AlignTimeAsync();
		}

		return DateTimeOffset.UtcNow.ToUnixTimeSeconds() + _timeDifference;
	}

	internal class TimeQuery
	{
		[JsonProperty("response")] internal TimeQueryResponse Response { get; set; }

		internal class TimeQueryResponse
		{
			[JsonProperty("server_time")] public long ServerTime { get; set; }
		}
	}
}