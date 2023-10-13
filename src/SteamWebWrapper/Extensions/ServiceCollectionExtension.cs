using Microsoft.Extensions.DependencyInjection;

namespace SteamWebWrapper.Extensions;

public static class ServiceCollectionExtensions
{
    public static void NetworkDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IHttpClientFactory>()
            .AddSingleton<IHttpMessageHandlerFactory>();
    }
}