using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Privacy;

public class Privacy
{
	[JsonPropertyName("eCommentPermission")]
	public PrivacyStatus ECommentPermission { get; set; }

	[JsonPropertyName("PrivacySettings")] public PrivacySettings PrivacySettings { get; set; }
}