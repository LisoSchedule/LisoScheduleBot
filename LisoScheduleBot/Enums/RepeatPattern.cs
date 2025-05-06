using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LisoScheduleBot.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum RepeatPattern
{
    None = 0,
    EveryWeek = 1,
    Every2Weeks = 2,
    Every4Weeks = 4,
    Every8Weeks = 8
}
