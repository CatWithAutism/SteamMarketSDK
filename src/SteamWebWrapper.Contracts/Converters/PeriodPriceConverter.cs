using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using SteamWebWrapper.Common.Utils;
using SteamWebWrapper.Contracts.Entities.Market.PriceHistory;

namespace SteamWebWrapper.Contracts.Converters;

internal class PeriodPriceConverter : JsonConverter<List<PriceHistoryPeriodPrice>>
{
    public override bool CanConvert(Type t) => t == typeof(List<PriceHistoryPeriodPrice>);

    public override List<PriceHistoryPeriodPrice> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var value = jsonDoc.RootElement.GetRawText();
        if (value.IsNullOrEmpty() || value == "[]")
        {
            return new List<PriceHistoryPeriodPrice>();
        }
        
        var data = JsonSerializer.Deserialize<List<List<object>>>(value, options);
        if (data == null)
        {
            throw new ArgumentException($"We cannot serialize this data - {value}");
        }
        
        var periodPrices = data.Select(periodData => new PriceHistoryPeriodPrice
        {
            Period = DateTime.ParseExact(periodData[0].ToString(), "MMM dd yyyy HH: z", CultureInfo.InvariantCulture),
            Price = float.Parse(periodData[1].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture),
            Quantity = long.Parse(periodData[2].ToString()),
        }).ToList();
        
        return periodPrices;
    }

    public override void Write(Utf8JsonWriter writer, List<PriceHistoryPeriodPrice> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
    
}