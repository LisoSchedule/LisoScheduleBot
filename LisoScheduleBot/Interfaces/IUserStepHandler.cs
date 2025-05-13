using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Interfaces;

public interface IUserStepHandler
{
    UserStep Step { get; }
    Task Handle(Message message, User user);
}
