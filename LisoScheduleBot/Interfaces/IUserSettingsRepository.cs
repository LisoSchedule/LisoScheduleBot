using LisoScheduleBot.Models;

namespace LisoScheduleBot.Interfaces;

public interface IUserSettingsRepository
{
    Task<List<UserSettings>> GetAll();
    Task<UserSettings?> Get(int userId);
    Task Save(UserSettings settings);
    Task Remove(int userId);
}
