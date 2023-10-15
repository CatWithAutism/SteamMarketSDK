using FluentAssertions;
using SteamWebWrapper.Core.Implementations;
using SteamWebWrapper.Core.Interfaces;
using SteamWebWrapper.Implementations;
using SteamWebWrapper.IntegrationTests.Fixtures;
using SteamWebWrapper.Interfaces;
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
        SteamHttpClient.SteamId.Should().NotBeNullOrEmpty();
        var inventory = await InventoryWrapper.GetInventory(SteamHttpClient.SteamId!, 730, 2, "english", 100);
        
        inventory.Should().NotBeNull();
        inventory.Success.Should().BeTrue();
        inventory.Assets.Should().NotBeNullOrEmpty();
        inventory.Descriptions.Should().NotBeNullOrEmpty();
    }
}