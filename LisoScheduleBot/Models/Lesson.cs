using LisoScheduleBot.Enums;
using Newtonsoft.Json;

namespace LisoScheduleBot.Models;

public class Lesson
{
    [JsonProperty("lesson_id")]
    public int LessonId { get; set; }

    [JsonProperty("classroom_id")]
    public int ClassroomId { get; set; }

    [JsonProperty("teacher_id")]
    public int TeacherId { get; set; }

    [JsonProperty("subject_id")]
    public int SubjectId { get; set; }

    [JsonProperty("start_time")]
    public DateTime StartTime { get; set; }

    [JsonProperty("duration")]
    public int Duration { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
