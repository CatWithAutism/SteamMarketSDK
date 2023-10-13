using System.Security.Cryptography;
using System.Text;

namespace SteamWebWrapper.Common.Utils;

public static class CryptoUtils
{
    public static byte[] HexToByteArray(string hex) 
    {
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }
    
    public static byte[] GetRsaEncryptedPassword(string publicKeyExp, string publicKeyMod, string password) 
    {
        // RSA Encryption.
        var rsa = new RSACryptoServiceProvider();
        var rsaParameters = new RSAParameters
        {
            Exponent = CryptoUtils.HexToByteArray(publicKeyExp),
            Modulus = CryptoUtils.HexToByteArray(publicKeyMod)
        };

        rsa.ImportParameters(rsaParameters);

        // Encrypt the password and convert it.
        var bytePassword = Encoding.ASCII.GetBytes(password);
        return rsa.Encrypt(bytePassword, false);
    }
    
    public static string ConvertToBase64String(byte[] data) => Convert.ToBase64String(data);
}