using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SteamWebWrapper.Core.Contracts.Constants;

namespace SteamWebWrapper.SteamGuard
{
    /// <summary>
    /// Class to help align system time with the Steam server time. Not super advanced; probably not taking some things into account that it should.
    /// Necessary to generate up-to-date codes. In general, this will have an error of less than a second, assuming Steam is operational.
    /// </summary>
    public class TimeAligner
    {
        private static bool _aligned;
        private static int _timeDifference;

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

        public static void AlignTime()
        {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                try
                {
                    string response = client.UploadString(SteamEndpoints.TwoFactorTimeQuery, "steamid=0");
                    TimeQuery query = JsonConvert.DeserializeObject<TimeQuery>(response);
                    _timeDifference = (int)(query.Response.ServerTime - currentTime);
                    _aligned = true;
                }
                catch (WebException)
                {
                }
            }
        }

        public static async Task AlignTimeAsync()
        {
            long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            WebClient client = new WebClient();
            try
            {
                client.Encoding = Encoding.UTF8;
                string response = await client.UploadStringTaskAsync(new Uri(SteamEndpoints.TwoFactorTimeQuery), "steamid=0");
                TimeQuery query = JsonConvert.DeserializeObject<TimeQuery>(response);
                _timeDifference = (int)(query.Response.ServerTime - currentTime);
                _aligned = true;
            }
            catch (WebException)
            {
            }
        }

        internal class TimeQuery
        {
            [JsonProperty("response")]
            internal TimeQueryResponse Response { get; set; }

            internal class TimeQueryResponse
            {
                [JsonProperty("server_time")]
                public long ServerTime { get; set; }
            }

        }
    }
}
