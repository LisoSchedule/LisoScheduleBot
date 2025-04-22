using LisoScheduleBot.Enums;
using LisoScheduleBot.Models;

namespace LisoScheduleBot.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllUsers();
    Task<User> GetOrCreateUser(long userId, string? username = null);
    Task SaveUser(User user);
}
