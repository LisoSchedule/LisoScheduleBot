using Newtonsoft.Json;

namespace LisoScheduleBot.Models;

public class VerificationCode
{
    [JsonProperty("code_id")]
    public int CodeId { get; set; }

    [JsonProperty("user_id")]
    public int UserId { get; set; }

    [JsonProperty("code")]
    public string? Code { get; set; }

    [JsonProperty("is_used")]
    public bool IsUsed { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
