using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers;

public class MessageHandler
{
    private readonly IUserService _userService;
    private readonly INotificationService _notificationService;

    public MessageHandler(IUserService userService, INotificationService notificationService)
    {
        _userService = userService;
        _notificationService = notificationService;
    }

    public async Task Handle(Message message, User user)
    {
        if (message.Text == $"{Emoji.Refresh} Почати заново")
        {
            if (user.Step >= UserStep.MainMenu) return;

            user.Nickname = string.Empty;
            user.GroupId = -1;
            user.Step = UserStep.StartOver;
            await _userService.SaveUser(user);
        }
        else if (message.Text == $"{Emoji.Gear} Налаштування")
        {
            if (user.Step < UserStep.MainMenu) return;

            user.Step = UserStep.ChangeSettings;
            await _userService.SaveUser(user);
        }
        else if (message.Text == $"{Emoji.OpenBook} Розклад")
        {
            if (user.Step < UserStep.MainMenu) return;

            user.Step = UserStep.ChooseSchedule;
            await _userService.SaveUser(user);
        }    
    }
}
