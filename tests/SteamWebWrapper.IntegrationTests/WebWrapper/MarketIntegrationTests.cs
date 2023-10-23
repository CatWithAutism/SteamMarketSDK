using FluentAssertions;
using SteamWebWrapper.Contracts.Entities.Market.CreateBuyOrder;
using SteamWebWrapper.Contracts.Entities.Market.PriceOverview;
using SteamWebWrapper.Core.Interfaces;
using SteamWebWrapper.Implementations;
using SteamWebWrapper.IntegrationTests.Fixtures;
using SteamWebWrapper.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace SteamWebWrapper.IntegrationTests.WebWrapper;

public class MarketIntegrationTests : IClassFixture<SteamHttpClientFixture>
{
    private  ITestOutputHelper TestOutputHelper { get; set; }
    private ISteamHttpClient SteamHttpClient { get; set; }

    private IMarketWrapper MarketWrapper { get; set; }

    private IInventoryWrapper InventoryWrapper { get; set; }
    
    public MarketIntegrationTests(SteamHttpClientFixture steamHttpClientFixture, ITestOutputHelper testOutputHelper)
    {
        TestOutputHelper = testOutputHelper;
        SteamHttpClient = steamHttpClientFixture.SteamHttpClient;
        InventoryWrapper = new InventoryWrapper(steamHttpClientFixture.SteamHttpClient);
        MarketWrapper = new MarketWrapper(steamHttpClientFixture.SteamHttpClient);
    }

    [Fact]
    public async Task CollectMarketAccountInfoTest()
    {
        var accountInfo = await MarketWrapper.CollectMarketAccountInfo(CancellationToken.None);

        accountInfo.Should().NotBeNull();
        accountInfo.Success.Should().Be(1);
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
    
    [Fact]
    public async Task CreateAndCancelBuyOrderTest()
    {
        var accountInfo = await MarketWrapper.CollectMarketAccountInfo(CancellationToken.None);

        accountInfo.Should().NotBeNull();
        accountInfo.Success.Should().Be(1);
        if (!accountInfo.MarketAllowed || accountInfo.WalletBalance < 300)
        {
            return;
        }
        
        const long offset = 0;
        const long count = 150;
        var marketHistory = await MarketWrapper.GetMyListings(offset, count, CancellationToken.None);
        
        marketHistory.Should().NotBeNull();
        marketHistory.Success.Should().BeTrue();
        
        const long appId = 730;
        const long priceTotal = 300;
        const long quantity = 1;
        const string marketHashName = "P250 | Sand Dune (Field-Tested)";
        if (marketHistory.BuyOrders.Count > 0)
        {
            marketHistory.Assets.Should().NotBeNullOrEmpty();

            var existingOrder = marketHistory.BuyOrders.FirstOrDefault(t => t.HashName == marketHashName);
            if (existingOrder != null)
            {
                var cancelExistingOrder = await MarketWrapper.CancelBuyOrder(existingOrder.BuyOrderId, CancellationToken.None);

                cancelExistingOrder.Should().NotBeNull();
                cancelExistingOrder.Success.Should().Be(1);

                return;
            }
        }

        var createBuyOrderRequest = new CreateBuyOrderRequest(appId, marketHashName, accountInfo.WalletCurrency, priceTotal, quantity);
        var buyOrder = await MarketWrapper.CreateBuyOrder(createBuyOrderRequest, CancellationToken.None);

        buyOrder.Should().NotBeNull();
        buyOrder.Success.Should().Be(1);
        
        var cancelBuyOrder = await MarketWrapper.CancelBuyOrder(buyOrder.BuyOrderId, CancellationToken.None);

        cancelBuyOrder.Should().NotBeNull();
        cancelBuyOrder.Success.Should().Be(1);
    }
}