using SteamMarketSDK.Contracts.Exceptions;
using SteamMarketSDK.Core.Contracts.Interfaces;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SteamMarketSDK.Implementations;

public class SteamConverter : ISteamConverter
{
	public T DeserializeObject<T>(string content)
	{
		try
		{
			return JsonSerializer.Deserialize<T>(content) ??
			       throw new BadSteamResponseDataException("We got null in steam's json response.");
		}
		catch (JsonException exception)
		{
			throw new BadSteamResponseDataException("We cannot deserialize steam response.", exception);
		}
	}
}