using Newtonsoft.Json;
using LisoScheduleBot.Enums;

namespace LisoScheduleBot.Models;

public class User
{
    [JsonProperty("user_id")]
    public int UserId { get; set; }

    [JsonProperty("chat_id")]
    public long ChatId { get; set; }

    [JsonProperty("username")]
    public string? Username { get; set; }

    [JsonProperty("nickname")]
    public string? Nickname { get; set; }

    [JsonProperty("group_id")]
    public int GroupId { get; set; }

    [JsonProperty("step")]
    public UserStep Step { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public UserSettings Settings { get; set; } = new UserSettings();
}
