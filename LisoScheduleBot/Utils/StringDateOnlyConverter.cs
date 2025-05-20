using Newtonsoft.Json;

namespace LisoScheduleBot.Utils;

public class StringDateOnlyConverter : JsonConverter<DateOnly>
{
    private const string _format = "yyyy-MM-dd";

    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var dateString = reader.Value?.ToString();
        return DateOnly.ParseExact(dateString!, _format);
    }

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(_format));
    }
}
