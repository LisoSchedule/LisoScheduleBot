using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LisoScheduleBot.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum RepeatPattern
{
    None,
    Daily,
    Weekly,
    Monthly
}
