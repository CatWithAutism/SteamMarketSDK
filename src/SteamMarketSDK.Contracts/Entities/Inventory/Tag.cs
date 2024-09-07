using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Inventory;

public class Tag
{
	[JsonPropertyName("category")] public string Category { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	[JsonPropertyName("color")]
	public string Color { get; set; }

	[JsonPropertyName("internal_name")] public string InternalName { get; set; }

	[JsonPropertyName("localized_category_name")]
	public string LocalizedCategoryName { get; set; }

	[JsonPropertyName("localized_tag_name")]
	public string LocalizedTagName { get; set; }
}