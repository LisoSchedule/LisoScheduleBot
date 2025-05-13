using LisoScheduleBot.Models;

namespace LisoScheduleBot.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllUsers();
    Task<User> GetOrCreateUser(long chatId, string? username = null);
    Task SaveUser(User user);
    Task RemoveUser(User user);
}
