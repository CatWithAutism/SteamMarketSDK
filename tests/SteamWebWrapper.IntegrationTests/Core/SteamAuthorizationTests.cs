using FluentAssertions;
using Microsoft.Extensions.Configuration;
using SteamWebWrapper.Core.Entities.Authorization;
using SteamWebWrapper.Core.Implementations;
using Xunit;

namespace SteamWebWrapper.IntegrationTests.Core;

public class SteamAuthorizationTests
{
    private IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .AddUserSecrets<SteamAuthorizationTests>()
        .Build();


    [Fact]
    public async Task TestAuth()
    {
        
        var steamHttpClient = new SteamHttpClient();
        var steamAuthCredentials = new SteamAuthCredentials()
        {
            Login = Configuration["username"] ?? throw new InvalidOperationException(),
            Password = Configuration["password"] ?? throw new InvalidOperationException(),
            MachineName = Configuration["machineName"] ?? $"PrettyPC-{Guid.NewGuid()}",
            TwoFactor = Configuration["twofactor"] ?? string.Empty,
        };

        var steamAuthResult = await steamHttpClient.Authorize(steamAuthCredentials, CancellationToken.None);
        
        steamAuthResult.Should().NotBeNull();
        steamAuthResult.Success.Should().BeTrue();
    }
}