using System.Text.Json;
using System.Text.Json.Serialization;
using SteamWebWrapper.Common.Utils;
using SteamWebWrapper.Contracts.Entities.Market.MyListings;

namespace SteamWebWrapper.Contracts.Converters;

internal class MyListingsAssetFlatter : JsonConverter<List<MyListingsAsset>>
{
    public override bool CanConvert(Type t) => t == typeof(List<MyListingsAsset>);

    public override List<MyListingsAsset> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);

        var value = jsonDoc.RootElement.GetRawText();
        if (value.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(value), "Value of reader is null or empty.");
        }
        
        var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, MyListingsAsset>>>>(value, options);
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

    public override void Write(Utf8JsonWriter writer, List<MyListingsAsset> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}