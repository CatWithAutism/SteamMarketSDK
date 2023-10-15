using FluentAssertions;
using SteamWebWrapper.Implementations;
using SteamWebWrapper.IntegrationTests.Fixtures;
using SteamWebWrapper.Interfaces;
using Xunit;

namespace SteamWebWrapper.IntegrationTests.WebWrapper;

public class SteamMarketTests : IClassFixture<SteamHttpClientFixture>
{
    private IMarketWrapper MarketWrapper { get; set; }

    public SteamMarketTests(SteamHttpClientFixture steamHttpClientFixture)
    {
        MarketWrapper = new MarketWrapper(steamHttpClientFixture.SteamHttpClient);
    }

    [Fact]
    public async Task GetMarketHistoryTest()
    {
        const long offset = 0;
        const long count = 150;
        var marketHistory = await MarketWrapper.GetMarketHistoryAsync(offset, count);
        
        marketHistory.Should().NotBeNull();
        marketHistory.Assets.Should().NotBeNullOrEmpty();
        marketHistory.Assets.Should().NotBeNullOrEmpty();
        marketHistory.Purchases.Should().NotBeNullOrEmpty();
        marketHistory.Listings.Should().NotBeNullOrEmpty();
        marketHistory.Events.Should().NotBeNullOrEmpty();
    }
}