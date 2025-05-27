using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LisoScheduleBot.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum TimeBeforeClass
{
    FifteenMinutes = 15,
    ThirtyMinutes = 30,
    FortyFiveMinutes = 45,
    OneHour = 60
}
