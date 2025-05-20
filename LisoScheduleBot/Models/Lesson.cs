using Newtonsoft.Json;
using LisoScheduleBot.Utils;

namespace LisoScheduleBot.Models;

public class Lesson
{
    [JsonProperty("lesson_id")]
    public int LessonId { get; set; }

    [JsonProperty("classroom_id")]
    public int ClassroomId { get; set; }

    [JsonProperty("teacher_id")]
    public int TeacherId { get; set; }

    [JsonProperty("group_id")]
    public int GroupId { get; set; }

    [JsonProperty("subject_id")]
    public int SubjectId { get; set; }

    [JsonProperty("duration")]
    public int Duration { get; set; }

    [JsonProperty("start_time")]
    [JsonConverter(typeof(StringTimeOnlyConverter))]
    public TimeOnly StartTime { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
