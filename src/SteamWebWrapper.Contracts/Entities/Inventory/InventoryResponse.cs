using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Inventory
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public class InventoryResponse
    {
        [JsonPropertyName("assets")]
        public List<InventoryAsset> Assets { get; set; }

        [JsonPropertyName("descriptions")]
        public List<InventoryAssetDescription> Descriptions { get; set; }

        [JsonPropertyName("more_items")]
        public long MoreItems { get; set; }

        [JsonPropertyName("last_assetid")]
        public long LastAssetId { get; set; }

        [JsonPropertyName("total_inventory_count")]
        public long TotalInventoryCount { get; set; }

        [JsonPropertyName("success")]
        public long Success { get; set; }

        [JsonPropertyName("rwgrsn")]
        public long Rwgrsn { get; set; }
    }
}
