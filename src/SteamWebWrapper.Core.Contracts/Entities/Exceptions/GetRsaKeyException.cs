using System.Net;

namespace SteamWebWrapper.Core.Contracts.Entities.Exceptions;

public class GetRsaKeyException : WebException
{
    public GetRsaKeyException(string message) : base(message)
    {
        
    }
}