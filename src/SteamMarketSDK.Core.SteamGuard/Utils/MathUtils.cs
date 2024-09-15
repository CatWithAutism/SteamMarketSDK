using System;
using System.Linq;

namespace SteamMarketSDK.SteamGuard.Utils;

public static class MathUtils
{
	public static string GetRandomHexNumber(int digits)
	{
		var random = new Random();
		var buffer = new byte[digits / 2];
		random.NextBytes(buffer);
		var result = string.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
		if (digits % 2 == 0)
		{
			return result;
		}

		return result + random.Next(16).ToString("X");
	}
}