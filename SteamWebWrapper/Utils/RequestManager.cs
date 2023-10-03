namespace SteamWebWrapper.Utils;

public class RequestHandler : IHttpClientFactory
{
    private static IHttpClientFactory ClientFactory = new DefaultHttpClientFactory();
    
    public HttpClient CreateClient(string name)
    {
        throw new NotImplementedException();
    }
}