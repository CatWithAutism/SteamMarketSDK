using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using SteamWebWrapper.Contracts.Entities.Authorization;
using SteamWebWrapper.Core.Implementations;
using SteamWebWrapper.Core.Interfaces;

namespace SteamWebWrapper.IntegrationTests.Fixtures;

public class SteamHttpClientFixture : IDisposable
{
    private IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .AddUserSecrets<SteamHttpClientFixture>()
        .Build();
    
    public SteamHttpClientFixture()
    {
        const string baseUrl = "https://steamcommunity.com/";
        
        var cookieContainer = new CookieContainer();

        var httpClientHandler = new HttpClientHandler
        {
            CookieContainer = cookieContainer,
            UseCookies = true,
            AutomaticDecompression = DecompressionMethods.All,
        };

        var steamHttpClient = new SteamHttpClient(baseUrl, httpClientHandler);
        var steamAuthCredentials = new SteamAuthCredentials()
        {
            Login = Configuration["username"] ?? throw new InvalidOperationException(),
            Password = Configuration["password"] ?? throw new InvalidOperationException(),
            MachineName = Configuration["machineName"] ?? $"PrettyPC-{Guid.NewGuid()}",
            TwoFactor = Configuration["twofactor"] ?? string.Empty,
        };

        var steamAuthResult = steamHttpClient.Authorize(steamAuthCredentials, CancellationToken.None).GetAwaiter().GetResult();
        
        steamAuthResult.Should().NotBeNull();
        steamAuthResult.Success.Should().BeTrue();

        SteamHttpClient = steamHttpClient;
    }
    
    public void Dispose()
    {
        SteamHttpClient.Dispose();
    }
    
    public ISteamHttpClient SteamHttpClient { get; private set; }
}