using SteamWebWrapper.Contracts.Exceptions;
using SteamWebWrapper.Core.Contracts.Interfaces;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SteamWebWrapper.Implementations;

public class SteamConvertor : ISteamConverter
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