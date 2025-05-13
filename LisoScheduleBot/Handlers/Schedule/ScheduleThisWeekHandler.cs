using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Schedule;

public class ScheduleThisWeekHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;

    public ScheduleThisWeekHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public UserStep Step => UserStep.ScheduleThisWeek;

    public async Task Handle(Message message, User user)
    {
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.Calendar} Обери день.",
            replyMarkup: KeyboardFactory.WeekButtons()
        );
    }
}
