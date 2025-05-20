using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LisoScheduleBot.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum RepeatType
{
    None,
    Daily,
    Weekly,
    Monthly
}
