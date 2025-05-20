using Newtonsoft.Json;
using LisoScheduleBot.Enums;

namespace LisoScheduleBot.Models;

public class Teacher
{
    [JsonProperty("teacher_id")]
    public int TeacherId { get; set; }

    [JsonProperty("full_name")]
    public string? FullName { get; set; }

    [JsonProperty("position")]
    public TeacherPosition Position { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
