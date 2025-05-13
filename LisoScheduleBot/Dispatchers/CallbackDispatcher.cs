using Telegram.Bot.Types;
using LisoScheduleBot.Interfaces;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Dispatchers;

public class CallbackDispatcher
{
    private readonly IEnumerable<ICallbackHandler> _handlers;

    public CallbackDispatcher(IEnumerable<ICallbackHandler> handlers)
    {
        _handlers = handlers;
    }

    public async Task Dispatch(CallbackQuery callbackQuery, User user)
    {
        foreach (var handler in _handlers)
        {
            if (handler.CanHandle(callbackQuery.Data!))
            {
                await handler.Handle(callbackQuery, user);
                return;
            }
        }
    }
}
