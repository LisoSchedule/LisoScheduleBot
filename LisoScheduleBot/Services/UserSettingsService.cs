using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;

namespace LisoScheduleBot.Services;

public class UserSettingsService : IUserSettingsService
{
    private readonly JsonUserSettingsRepository _settingsRepository;

    public UserSettingsService(JsonUserSettingsRepository settingsRepository)
    {
        _settingsRepository = settingsRepository;
    }

    public async Task<List<UserSettings>> GetAllUserSettings()
    {
        return await _settingsRepository.GetAll();
    }

    public async Task<UserSettings> GetOrCreateUserSettings(int userId)
    {
        var settings = await _settingsRepository.Get(userId);

        if (settings != null) return settings;

        settings = new UserSettings
        {
            UserId = userId,
            ReceiveNotifications = true,
            TimeBeforeClassToNotify = TimeSpan.FromMinutes(60),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return settings;
    }

    public async Task SaveUserSettings(UserSettings settings)
    {
        settings.UpdatedAt = DateTime.UtcNow;
        await _settingsRepository.Save(settings);
    }
}
