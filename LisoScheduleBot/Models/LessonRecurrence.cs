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

    [JsonProperty("repeatability")]
    public RepeatPattern Repeatability { get; set; }

    [JsonProperty("start_date")]
    [JsonConverter(typeof(StringDateOnlyConverter))]
    public DateOnly StartDate { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
