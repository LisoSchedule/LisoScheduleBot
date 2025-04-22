using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers;

public class MessageHandler
{
    private readonly IUserService _userService;

    public MessageHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task Handle(Message message, User user)
    {
        if (message.Text == "Почати заново")
        {
            user.Nickname = string.Empty;
            user.GroupId = -1;
            user.Step = UserStep.StartOver;
            await _userService.SaveUser(user);
        }
    }
}
