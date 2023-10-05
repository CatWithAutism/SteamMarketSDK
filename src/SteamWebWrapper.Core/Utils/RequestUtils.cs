using System.Net.Http.Json;
using SteamWebWrapper.Core.Interfaces;

namespace SteamWebWrapper.Core.Utils;

internal static class RequestUtils
{
    /// <summary>
    /// Execute Get request depending on <see cref="T"/>. 
    /// </summary>
    /// <typeparam name="T">Deserialized object or type presented by HttpClientGet</typeparam>
    /// <returns>Returns deserialized object or type presented by HttpClientGet</returns>
    public static async Task<T?> Get<T>(this HttpClient httpClient, Uri uri, CancellationToken cancellationToken) where T : class
    {
        var typeObject = typeof(T);
        if (typeObject == typeof(string))
        {
            return await httpClient.GetStringAsync(uri, cancellationToken) as T;
        }

        if (typeObject == typeof(Stream))
        {
            return await httpClient.GetStreamAsync(uri, cancellationToken) as T;
        }

        if (typeObject == typeof(byte[]))
        {
            return await httpClient.GetByteArrayAsync(uri, cancellationToken) as T;
        }
        
        return await httpClient.GetFromJsonAsync<T>(uri, cancellationToken);
    }
    

}