using System.Text.Json;
using System.Text.Json.Serialization;
using SteamWebWrapper.Common.Utils;
using SteamWebWrapper.Contracts.Entities.Market.History;

namespace SteamWebWrapper.Contracts.Converters;

internal class AssetFlatter : JsonConverter<List<MarketHistoryAsset>>
{
    public override bool CanConvert(Type t) => t == typeof(List<MarketHistoryAsset>);

    public override List<MarketHistoryAsset> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);

        var value = jsonDoc.RootElement.GetRawText();
        if (value.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(value), $"Value of reader is null or empty.");
        }
        
        var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, MarketHistoryAsset>>>>(value, options);
        if (data == null)
        {
            throw new ArgumentException($"We cannot serialize this data - {value}");
        }
        
        return data
            .SelectMany(
                gameContext => gameContext.Value.SelectMany(
                    contextAsset => contextAsset.Value
                        .Select(pair => pair.Value))).ToList();
    }

    public override void Write(Utf8JsonWriter writer, List<MarketHistoryAsset> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}