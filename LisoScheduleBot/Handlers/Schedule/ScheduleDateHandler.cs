using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Schedule;

public class ScheduleDateHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;

    public ScheduleDateHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public UserStep Step => UserStep.ScheduleDate;

    public async Task Handle(Message message, User user)
    {
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.Date} Розклад на сьогодні ({DateTime.UtcNow:dd.MM.yy}).",
            replyMarkup: KeyboardFactory.DateBack(DateOnly.FromDateTime(DateTime.UtcNow))
        );
    }
}
