using FluentAssertions;
using SteamWebWrapper.Core.Interfaces;
using SteamWebWrapper.IntegrationTests.Fixtures;
using Xunit;

namespace SteamWebWrapper.IntegrationTests.WebWrapper;

public class SteamAccountTests : IClassFixture<SteamHttpClientFixture>
{
    private ISteamHttpClient SteamHttpClient { get; set; }

    public SteamAccountTests(SteamHttpClientFixture steamHttpClientFixture)
    {
        SteamHttpClient = steamHttpClientFixture.SteamHttpClient;
    }
    
    [Fact]
    public async Task GetAccountInfoTest()
    {
        var accountInfo = await SteamHttpClient.GetAccountInfoAsync(CancellationToken.None);
        
        accountInfo.Should().NotBeNull();
        accountInfo.Success.Should().Be(1);
        accountInfo.SteamId.Should().NotBeNullOrEmpty();
    }
}