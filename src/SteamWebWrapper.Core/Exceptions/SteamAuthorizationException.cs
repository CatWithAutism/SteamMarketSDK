using System.Net;

namespace SteamWebWrapper.Core.Exceptions;

public class SteamAuthorizationException : WebException
{
    public SteamAuthorizationException(string message) : base(message)
    {
        
    }
}