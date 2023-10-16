using System.Net;
using System.Text.Json;
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
        
        if (bool.TryParse(Configuration["useCookie"], out var useCookie) && useCookie)
        {
            var serializedCookieCollection = Configuration["cookieCollection"] ?? throw new InvalidOperationException();
            var cookieCollection = JsonSerializer.Deserialize<CookieCollection>(serializedCookieCollection) ?? throw new InvalidOperationException();
            cookieContainer.Add(cookieCollection);
        }
        
        var httpClientHandler = new HttpClientHandler
        {
            CookieContainer = cookieContainer,
            UseCookies = true,
            AutomaticDecompression = DecompressionMethods.All,
        };

        var steamHttpClientHandler = new SteamHttpClientHandler(httpClientHandler);
        
        if (useCookie)
        {
            SteamHttpClient = new SteamHttpClient(baseUrl, steamHttpClientHandler);
            return;
        }

        var steamHttpClient = new SteamHttpClient(baseUrl, steamHttpClientHandler);
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