using FluentAssertions;
using SteamWebWrapper.Contracts.Entities.Account;
using SteamWebWrapper.Contracts.Entities.Market.PriceOverview;
using SteamWebWrapper.Core.Interfaces;
using SteamWebWrapper.Implementations;
using SteamWebWrapper.IntegrationTests.Fixtures;
using Xunit;

namespace SteamWebWrapper.IntegrationTests.WebWrapper;

public class SteamIntegrationTests : IClassFixture<SteamHttpClientFixture>
{
    private ISteamHttpClient SteamHttpClient { get; set; }

    private MarketWrapper MarketWrapper { get; set; }

    private InventoryWrapper InventoryWrapper { get; set; }

    private AccountInfo? AccountInfo { get; set; }
    
    public SteamIntegrationTests(SteamHttpClientFixture steamHttpClientFixture)
    {
        SteamHttpClient = steamHttpClientFixture.SteamHttpClient;
        InventoryWrapper = new InventoryWrapper(steamHttpClientFixture.SteamHttpClient);
        MarketWrapper = new MarketWrapper(steamHttpClientFixture.SteamHttpClient);
    }

    [Fact]
    public async Task CollectMarketAccountInfoTest()
    {
        AccountInfo = await MarketWrapper.CollectMarketAccountInfo(CancellationToken.None);

        AccountInfo.Should().NotBeNull();
        AccountInfo.Success.Should().Be(1);
    }
    
    [Fact]
    public async Task GetInventoryTest()
    {
        var inventory = await InventoryWrapper.GetInventory(SteamHttpClient.SteamId.ConvertToUInt64().ToString(), 730, 2, "english", 100);
        
        inventory.Should().NotBeNull();
        inventory.Success.Should().Be(1);
        inventory.Assets.Should().NotBeNullOrEmpty();
        inventory.Descriptions.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task GetMarketHistoryTest()
    {
        const long offset = 0;
        const long count = 150;
        var marketHistory = await MarketWrapper.GetMarketHistoryAsync(offset, count, CancellationToken.None);
        
        marketHistory.Should().NotBeNull();
        marketHistory.Assets.Should().NotBeNullOrEmpty();
        marketHistory.Assets.Should().NotBeNullOrEmpty();
        marketHistory.Purchases.Should().NotBeNullOrEmpty();
        marketHistory.Listings.Should().NotBeNullOrEmpty();
        marketHistory.Events.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public async Task GetMyListingsHistoryTest()
    {
        const long offset = 0;
        const long count = 150;
        var marketHistory = await MarketWrapper.GetMyListings(offset, count, CancellationToken.None);
        
        marketHistory.Should().NotBeNull();
        marketHistory.Success.Should().BeTrue();
        if (marketHistory.Listings.Count > 0)
        {
            marketHistory.Assets.Should().NotBeNullOrEmpty();
        }
    }
    
    [Fact]
    public async Task GetByOrderStatusTest()
    {
        const long offset = 0;
        const long count = 150;
        var marketHistory = await MarketWrapper.GetMyListings(offset, count, CancellationToken.None);

        marketHistory.Should().NotBeNull();
        marketHistory.Success.Should().BeTrue();

        if (marketHistory.BuyOrders is { Count: > 0 })
        {
            var buyOrderIndex = new Random().Next(0, marketHistory.BuyOrders.Count - 1);
            var buyOrderId = marketHistory.BuyOrders.ElementAt(buyOrderIndex).BuyOrderId;
            var buyOrderStatus = await MarketWrapper.GetBuyOrderStatus(buyOrderId, CancellationToken.None);

            buyOrderStatus.Should().NotBeNull();
            buyOrderStatus.Success.Should().Be(1);
            buyOrderStatus.Active.Should().Be(1);
        }
    }
    
    [Fact]
    public async Task GetPriceOverviewTest()
    {
        const string country = "US";
        const long appId = 730;
        const string marketHashName = "P250 | Sand Dune (Field-Tested)";
        const long currency = 5;

        var priceRequest = new PriceOverviewRequest(appId, marketHashName, country, currency);
        var priceResponse = await MarketWrapper.GetItemCurrentPrice(priceRequest, CancellationToken.None);

        priceResponse.Should().NotBeNull();
        priceResponse.Success.Should().BeTrue();
    }
}