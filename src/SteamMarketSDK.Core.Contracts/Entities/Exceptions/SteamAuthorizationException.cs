using System.Net;

namespace SteamMarketSDK.Core.Contracts.Entities.Exceptions;

public class SteamAuthorizationException : WebException
{
	public SteamAuthorizationException(string message) : base(message)
	{
	}
}