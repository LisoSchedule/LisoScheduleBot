using LisoScheduleBot.Enums;
using Newtonsoft.Json;

namespace LisoScheduleBot.Models;

public class Subject
{
    [JsonProperty("subject_id")]
    public int SubjectId { get; set; }

    [JsonProperty("subject_type")]
    public SubjectType Type { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
