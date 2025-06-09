using LisoScheduleBot.Models;

namespace LisoScheduleBot.Interfaces;

public interface ICodeService
{
    Task<VerificationCode> GetOrCreateLastCode(int userId);
}
