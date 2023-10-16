using FluentAssertions;
using SteamWebWrapper.Core.Interfaces;
using SteamWebWrapper.Implementations;
using SteamWebWrapper.IntegrationTests.Fixtures;
using Xunit;

namespace SteamWebWrapper.IntegrationTests.WebWrapper;

public class SteamInventoryTests : IClassFixture<SteamHttpClientFixture>
{
    private InventoryWrapper InventoryWrapper { get; set; }
    private ISteamHttpClient SteamHttpClient { get; set; }

    public SteamInventoryTests(SteamHttpClientFixture steamHttpClientFixture)
    {
        SteamHttpClient = steamHttpClientFixture.SteamHttpClient;
        InventoryWrapper = new InventoryWrapper(steamHttpClientFixture.SteamHttpClient);
    }

    [Fact]
    public async Task GetInventoryTest()
    {
        var accountInfo = await SteamHttpClient.GetAccountInfoAsync(CancellationToken.None);
        accountInfo.Should().NotBeNull();
        
        var inventory = await InventoryWrapper.GetInventory(accountInfo!.SteamId, 730, 2, "english", 100);
        
        inventory.Should().NotBeNull();
        inventory!.Success.Should().Be(1);
        inventory.Assets.Should().NotBeNullOrEmpty();
        inventory.Descriptions.Should().NotBeNullOrEmpty();
    }
}