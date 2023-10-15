using System.Text.Json.Serialization;

namespace SteamWebWrapper.Contracts.Entities.Privacy;

public class Privacy
{
    [JsonPropertyName("PrivacySettings")]
    public PrivacySettings PrivacySettings { get; set; }

    [JsonPropertyName("eCommentPermission")]
    public PrivacyStatus ECommentPermission { get; set; }
}