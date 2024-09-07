using System.Net;

namespace SteamMarketSDK.Core.Contracts.Entities.Exceptions;

public class GetRsaKeyException : WebException
{
	public GetRsaKeyException(string message) : base(message)
	{
	}
}