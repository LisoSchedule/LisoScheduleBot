using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using LisoScheduleBot.Config;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;

namespace LisoScheduleBot.Repositories;

public class JsonUserSettingsRepository : IUserSettingsRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerSettings _settings;

    public JsonUserSettingsRepository(AppConfig config)
    {
        _filePath = config.UserSettingsJson;
        _settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = { new StringEnumConverter() }
        };
    }

    public async Task<List<UserSettings>> GetAll()
    {
        return await LoadUserSettings();
    }

    public async Task<UserSettings?> Get(int userId)
    {
        var allSettings = await LoadUserSettings();
        return allSettings.FirstOrDefault(s => s.UserId == userId);
    }

    public async Task Save(UserSettings settings)
    {
        var allSettings = await LoadUserSettings();
        var existingSettings = allSettings.FirstOrDefault(s => s.UserId == settings.UserId);

        if (existingSettings != null) allSettings.Remove(existingSettings);

        allSettings.Add(settings);
        await SaveChanges(allSettings);
    }

    public async Task Remove(int userId)
    {
        var allSettings = await LoadUserSettings();
        var existingSettings = allSettings.FirstOrDefault(s => s.UserId == userId);

        if (existingSettings != null)
        {
            allSettings.Remove(existingSettings);
            await SaveChanges(allSettings);
        }
    }

    private async Task<List<UserSettings>> LoadUserSettings()
    {
        if (!File.Exists(_filePath)) return new List<UserSettings>();

        var json = await File.ReadAllTextAsync(_filePath);
        return JsonConvert.DeserializeObject<List<UserSettings>>(json, _settings) ?? new List<UserSettings>();
    }

    private async Task SaveChanges(List<UserSettings> settings)
    {
        var json = JsonConvert.SerializeObject(settings, _settings);
        await File.WriteAllTextAsync(_filePath, json);
    }
}
