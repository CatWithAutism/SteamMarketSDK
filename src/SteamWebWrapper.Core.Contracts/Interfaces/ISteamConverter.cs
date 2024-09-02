namespace SteamWebWrapper.Core.Contracts.Interfaces;

public interface ISteamConverter
{
	T DeserializeObject<T>(string content);
}