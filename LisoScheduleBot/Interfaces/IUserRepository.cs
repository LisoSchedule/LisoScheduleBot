using LisoScheduleBot.Models;

namespace LisoScheduleBot.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAll();
    Task<User?> Get(long chatId);
    Task Save(User user);
    Task Remove(int userId);
}
