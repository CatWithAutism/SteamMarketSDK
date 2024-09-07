using System.Security.Cryptography;
using System.Text;

namespace SteamMarketSDK.Common.Utils;

public static class CryptoUtils
{
	public static string ConvertToBase64String(byte[] data) => Convert.ToBase64String(data);

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

	public static byte[] GetRsaEncryptedPassword(string publicKeyExp, string publicKeyMod, string password)
	{
		// RSA Encryption.
		var rsa = new RSACryptoServiceProvider();
		var rsaParameters = new RSAParameters
		{
			Exponent = HexToByteArray(publicKeyExp), Modulus = HexToByteArray(publicKeyMod)
		};

		rsa.ImportParameters(rsaParameters);

		// Encrypt the password and convert it.
		var bytePassword = Encoding.ASCII.GetBytes(password);
		return rsa.Encrypt(bytePassword, false);
	}

	public static byte[] HexToByteArray(string hex) =>
		Enumerable.Range(0, hex.Length)
			.Where(x => x % 2 == 0)
			.Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
			.ToArray();
}