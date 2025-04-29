using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LisoScheduleBot.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum TimeBeforeClass
{
    FifteenMinutes = 15,
    ThirtyMinutes = 30,
    OneHour = 60
}
