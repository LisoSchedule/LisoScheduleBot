using LisoScheduleBot.Models;

namespace LisoScheduleBot.Interfaces;

public interface IUserRepository
{
    Task<User?> Get(long chatId);
}
