using System.Text.Json.Serialization;

namespace SteamMarketSDK.Contracts.Entities.Privacy;

public class Privacy
{
	[JsonPropertyName("eCommentPermission")]
	public PrivacyStatus ECommentPermission { get; set; }

	[JsonPropertyName("PrivacySettings")] public PrivacySettings PrivacySettings { get; set; }
}