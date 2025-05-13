using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Dispatchers;

public class UserStepDispatcher
{
    private readonly Dictionary<UserStep, IUserStepHandler> _handlers;

    public UserStepDispatcher(IEnumerable<IUserStepHandler> handlers)
    {
        _handlers = handlers.ToDictionary(handler => handler.Step);
    }

    public async Task Dispatch(Message message, User user)
    {
        if (_handlers.TryGetValue(user.Step, out var handler))
        {
            await handler.Handle(message, user);
        }
        else
        {
            Console.WriteLine($"No handler found for step: {user.Step}");
        }
    }
}
