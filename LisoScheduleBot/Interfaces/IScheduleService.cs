using LisoScheduleBot.Models;

namespace LisoScheduleBot.Interfaces
{
    public interface IScheduleService
    {
        Task<List<ScheduleItem>> GetScheduleDetailsByDate(DateOnly date);
    }
}
