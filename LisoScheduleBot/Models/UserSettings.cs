using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LisoScheduleBot.Enums;

namespace LisoScheduleBot.Models;

public class UserSettings
{
    [JsonProperty("settings_id")]
    public int SettingsId { get; set; }

    [JsonProperty("user_id")]
    public int UserId { get; set; }

    [JsonProperty("receive_notifications")]
    public bool ReceiveNotifications { get; set; }

    [JsonProperty("time_before_class_to_notify")]
    [JsonConverter(typeof(StringEnumConverter))]
    public TimeBeforeClass TimeBeforeClassToNotify { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
