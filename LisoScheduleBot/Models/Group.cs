using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LisoScheduleBot.Enums;

namespace LisoScheduleBot.Models;

public class Group
{
    [JsonProperty("group_id")]
    public int GroupId { get; set; }

    [JsonProperty("sub_group")]
    public int SubGroup { get; set; }

    [JsonProperty("name")]
    [JsonConverter(typeof(StringEnumConverter))]
    public GroupName Name { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
