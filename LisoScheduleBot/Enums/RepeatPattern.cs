using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LisoScheduleBot.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum RepeatPattern
{
    EveryOneWeek = 1,
    EveryTwoWeeks = 2,
    EveryFourWeeks = 4
}
