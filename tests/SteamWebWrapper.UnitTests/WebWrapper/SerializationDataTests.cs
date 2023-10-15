using System.Text.Json;
using FluentAssertions;
using SteamWebWrapper.Entities.Market.History;
using Xunit;

namespace SteamWebWrapper.UnitTests.WebWrapper;

public class SerializationDataTests
{
    [Fact]
    public async Task DeserializeMarketHistoryResponseTest()
    {
        const int assetCount = 135;
        const int eventCount = 150;
        const int purchaseCount = 110;
        const string dataPath = "Data/MarketHistoryResponseTestData.json";
        
        var data = await File.ReadAllTextAsync(dataPath);
        data.Should().NotBeNullOrEmpty();

        var marketHistoryResponse = JsonSerializer.Deserialize<MarketHistoryResponse>(data);
        marketHistoryResponse.Should().NotBeNull();
        marketHistoryResponse.Assets.Should().NotBeNullOrEmpty();
        marketHistoryResponse.Assets.Count.Should().Be(assetCount);
        marketHistoryResponse.Purchases.Count.Should().Be(purchaseCount);
        marketHistoryResponse.Listings.Count.Should().Be(assetCount);
        marketHistoryResponse.Events.Count.Should().Be(eventCount);
    }
}