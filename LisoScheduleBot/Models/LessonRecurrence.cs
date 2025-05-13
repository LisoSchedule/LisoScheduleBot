using LisoScheduleBot.Enums;
using LisoScheduleBot.Utils;
using Newtonsoft.Json;

namespace LisoScheduleBot.Models;

public class LessonRecurrence
{
    [JsonProperty("recurrence_id")]
    public int RecurrenceId { get; set; }

    [JsonProperty("lesson_id")]
    public int LessonId { get; set; }

    [JsonProperty("repeat_type")]
    public RepeatPattern RepeatType { get; set; }

    [JsonProperty("repeat_value")]
    public int RepeatValue { get; set; }

    [JsonProperty("start_date")]
    [JsonConverter(typeof(StringDateOnlyConverter))]
    public DateOnly StartDate { get; set; }

    [JsonProperty("end_date")]
    [JsonConverter(typeof(StringDateOnlyConverter))]
    public DateOnly EndDate { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
