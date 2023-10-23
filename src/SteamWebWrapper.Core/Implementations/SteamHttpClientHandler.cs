namespace SteamWebWrapper.Core.Implementations;

public class SteamHttpClientHandler : DelegatingHandler
{
    public string? SteamCountry { get; private set; }
    public string? SessionId { get; private set; }
    public string? SteamLoginSecure { get; private set; }
    
    public SteamHttpClientHandler(HttpClientHandler clientHandler) : base(clientHandler)
    {
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var responseMessage = base.Send(request, cancellationToken);
        UpdateSessionData(request.RequestUri!);
        
        return responseMessage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var responseMessage = await base.SendAsync(request, cancellationToken);
        UpdateSessionData(request.RequestUri!);
        
        return responseMessage;
    }

    private void UpdateSessionData(Uri uri)
    {
        if (InnerHandler is not HttpClientHandler httpClientHandler)
        {
            throw new InvalidOperationException($"We cannot convert {nameof(InnerHandler)} to {nameof(HttpClientHandler)}");
        }

        var cookieCollection = httpClientHandler.CookieContainer.GetCookies(uri);
        SteamCountry = cookieCollection["steamCountry"]?.Value;
        SessionId = cookieCollection["sessionid"]?.Value;
        SteamLoginSecure = cookieCollection["steamLoginSecure"]?.Value;
    }
}