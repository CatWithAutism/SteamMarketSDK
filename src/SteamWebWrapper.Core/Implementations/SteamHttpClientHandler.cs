namespace SteamWebWrapper.Core.Implementations;

public class SteamHttpClientHandler : DelegatingHandler
{


    
    public SteamHttpClientHandler(HttpClientHandler clientHandler) : base(clientHandler)
    {
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var responseMessage = base.Send(request, cancellationToken);
        
        return responseMessage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var responseMessage = await base.SendAsync(request, cancellationToken);
        
        return responseMessage;
    }
}