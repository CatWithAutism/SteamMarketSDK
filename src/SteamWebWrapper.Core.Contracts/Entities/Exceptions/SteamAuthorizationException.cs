using System.Net;

namespace SteamWebWrapper.Core.Contracts.Entities.Exceptions;

public class SteamAuthorizationException : WebException
{
	public SteamAuthorizationException(string message) : base(message)
	{
	}
}