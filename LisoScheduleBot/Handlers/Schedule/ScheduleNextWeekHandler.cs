using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Schedule;

public class ScheduleNextWeekHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;

    public ScheduleNextWeekHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public UserStep Step => UserStep.ScheduleNextWeek;

    public async Task Handle(Message message, User user)
    {
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.Calendar} Обери день.",
            replyMarkup: KeyboardFactory.NextWeekButtons()
        );
    }
}
