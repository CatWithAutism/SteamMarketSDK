using System.Text.Json;
using System.Text.Json.Serialization;
using SteamWebWrapper.Common.Utils;

namespace SteamWebWrapper.Contracts.Converters;

internal class ListFromDictionaryConverter<T> : JsonConverter<List<T>>
{
    public override bool CanConvert(Type t) => t == typeof(List<T>);

    public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);

        var value = jsonDoc.RootElement.GetRawText();
        if (value.IsNullOrEmpty() || value == "[]")
        {
            return new List<T>();
        }
        
        var data = JsonSerializer.Deserialize<Dictionary<string, T>>(value, options);
        if (data == null)
        {
            throw new ArgumentException($"We cannot serialize this data - {value}");
        }

        return data.Select(pair => pair.Value).ToList();
    }

    public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}