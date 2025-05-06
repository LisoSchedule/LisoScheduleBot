using Newtonsoft.Json;

namespace LisoScheduleBot.Models;

public class Classroom
{
    [JsonProperty("classroom_id")]
    public int ClassroomId { get; set; }

    [JsonProperty("hull")]
    public string? Hull { get; set; }

    [JsonProperty("room")]
    public string? Room { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
