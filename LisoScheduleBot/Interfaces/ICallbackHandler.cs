using Telegram.Bot.Types;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Interfaces;

public interface ICallbackHandler
{
    bool CanHandle(string callbackData);
    Task Handle(CallbackQuery callbackQuery, User user);
}
