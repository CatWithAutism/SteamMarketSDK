using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using SteamWebWrapper.Common.Utils;
using SteamWebWrapper.Contracts.Entities.Authorization;
using SteamWebWrapper.Core.Implementations;
using SteamWebWrapper.Core.Interfaces;

namespace SteamWebWrapper.IntegrationTests.Fixtures;

public class SteamHttpClientFixture : IDisposable
{
        
    public ISteamHttpClient SteamHttpClient { get; private set; }
    
    private IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .AddUserSecrets<SteamHttpClientFixture>()
        .AddEnvironmentVariables()
        .Build();
    
    public SteamHttpClientFixture()
    {
        var serializedSteamGuard = Configuration["steamGuard"];
        ISteamGuardAuthenticator? steamGuardAuthenticator = JsonSerializer.Deserialize<SteamGuardAuthenticator>(serializedSteamGuard) ?? null;
        
        var httpClientHandler = new HttpClientHandler
        {
            CookieContainer = new CookieContainer(),
            UseCookies = true,
            AutomaticDecompression = DecompressionMethods.All,
        };

        var steamHttpClientHandler = new SteamHttpClientHandler(httpClientHandler);

        var steamHttpClient = new SteamHttpClient(steamHttpClientHandler);
        var steamAuthCredentials = new SteamAuthCredentials()
        {
            Login = Configuration["username"] ?? throw new InvalidOperationException(),
            Password = Configuration["password"] ?? throw new InvalidOperationException(),
            MachineName = Configuration["machineName"] ?? $"PrettyPC-{Guid.NewGuid()}",
        };

        steamHttpClient.AuthorizeViaOAuth(steamAuthCredentials, steamGuardAuthenticator, CancellationToken.None).GetAwaiter().GetResult();

        SteamHttpClient = steamHttpClient;
    }
    
    public void Dispose()
    {
        SteamHttpClient.Dispose();
    }
}