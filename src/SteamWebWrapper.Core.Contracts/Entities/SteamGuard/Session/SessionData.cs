using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SteamWebWrapper.Core.Contracts.Entities.SteamGuard.Session
{
    public class SessionData
    {
        public ulong SteamId { get; set; }

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public string? SessionId { get; set; }
    }
}
