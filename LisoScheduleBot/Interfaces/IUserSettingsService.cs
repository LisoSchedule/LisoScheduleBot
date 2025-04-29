using LisoScheduleBot.Models;

namespace LisoScheduleBot.Interfaces;

public interface IUserSettingsService
{
    Task<List<UserSettings>> GetAllUserSettings();
    Task<UserSettings> GetOrCreateUserSettings(int userId);
    Task SaveUserSettings(UserSettings settings);
}
