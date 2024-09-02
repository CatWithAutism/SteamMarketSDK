namespace SteamWebWrapper.Common.Utils;

public static class CollectionUtils
{
	public static bool IsNullOrEmpty<T>(this IEnumerable<T>? enumerable) => enumerable == null || !enumerable.Any();
}