using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LisoScheduleBot.Config;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;

namespace LisoScheduleBot.Repositories;

public class JsonGroupRepository : IGroupRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerSettings _settings;

    public JsonGroupRepository(AppConfig config)
    {
        _filePath = config.GroupsJson;
        _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { new StringEnumConverter() }
        };
    }

    public async Task<List<Group>> GetAll()
    {
        return await LoadGroups();
    }

    public async Task<Group?> Get(int groupId)
    {
        var groups = await LoadGroups();
        return groups.FirstOrDefault(g => g.GroupId == groupId);
    }

    public async Task Save(Group group)
    {
        var groups = await LoadGroups();
        var existingGroup = groups.FirstOrDefault(g => g.GroupId == group.GroupId);

        if (existingGroup != null)
        {
            groups.Remove(existingGroup);
        }

        groups.Add(group);
        await SaveChanges(groups);
    }

    private async Task<List<Group>> LoadGroups()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Group>();
        }

        var json = await File.ReadAllTextAsync(_filePath);
        return JsonConvert.DeserializeObject<List<Group>>(json, _settings) ?? new List<Group>();
    }

    private async Task SaveChanges(List<Group> groups)
    {
        var json = JsonConvert.SerializeObject(groups, _settings);
        await File.WriteAllTextAsync(_filePath, json);
    }
}
