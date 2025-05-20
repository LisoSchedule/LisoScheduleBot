using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;

namespace LisoScheduleBot.Services;

public class JsonUserSettingsService : IService<UserSettings>
{
    private readonly JsonUserSettingsRepository _settingsRepository;

    public JsonUserSettingsService(JsonUserSettingsRepository settingsRepository)
    {
        _settingsRepository = settingsRepository;
    }

    public async Task<List<UserSettings>> GetAllEntities()
    {
        return await _settingsRepository.GetAll();
    }

    public async Task<UserSettings> GetOrCreateEntity(int userId)
    {
        var settings = await _settingsRepository.Get(userId);

        if (settings != null) return settings;

        var allSettings = await _settingsRepository.GetAll();

        settings = new UserSettings
        {
            SettingsId = GetNextUserSettignsId(allSettings),
            UserId = userId,
            ReceiveNotifications = true,
            TimeBeforeClassToNotify = TimeBeforeClass.OneHour,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return settings;
    }

    public async Task SaveEntity(UserSettings settings)
    {
        settings.UpdatedAt = DateTime.UtcNow;
        await _settingsRepository.Save(settings);
    }

    public async Task RemoveEntity(int userId)
    {
        await _settingsRepository.Remove(userId);
    }

    private int GetNextUserSettignsId(List<UserSettings> settings)
    {
        return settings.Any() ? settings.Max(u => u.UserId) + 1 : 1;
    }
}
