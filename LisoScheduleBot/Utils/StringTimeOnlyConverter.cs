using Newtonsoft.Json;

namespace LisoScheduleBot.Utils;

public class StringTimeOnlyConverter : JsonConverter<TimeOnly>
{
    private const string _format = "HH:mm";

    public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var timeString = reader.Value?.ToString();
        return TimeOnly.ParseExact(timeString!, _format);
    }

    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(_format));
    }
}
