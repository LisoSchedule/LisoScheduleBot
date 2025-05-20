using LisoScheduleBot.Models;

namespace LisoScheduleBot.Interfaces;

public interface IScheduleService
{
    Task<List<ScheduleItem>> GetScheduleItemsByDate(DateOnly date, User user);
}
