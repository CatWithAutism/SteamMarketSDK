using System.Net;

namespace SteamWebWrapper.Core.Exceptions;

public class GetRsaKeyException : WebException
{
    public GetRsaKeyException(string message) : base(message)
    {
        
    }
}