using System.Text.Json;
using FluentAssertions;
using SteamWebWrapper.Contracts.Entities.Inventory;
using Xunit;

namespace SteamWebWrapper.UnitTests.WebWrapper;

public class InventorySerializationDataTests
{
    [Fact]
    public async Task DeserializeInventoryResponseTest()
    {
        const int assetCount = 87;
        const int descriptionCount = 71;
        const string dataPath = "Data/InventoryItemsResponseTestData.json";
        
        var data = await File.ReadAllTextAsync(dataPath);
        data.Should().NotBeNullOrEmpty();

        var inventoryResponse = JsonSerializer.Deserialize<InventoryResponse>(data);
        inventoryResponse.Should().NotBeNull();
        inventoryResponse.Assets.Should().NotBeNullOrEmpty();
        inventoryResponse.Assets.Count.Should().Be(assetCount);
        inventoryResponse.Descriptions.Count.Should().Be(descriptionCount);
    }
}