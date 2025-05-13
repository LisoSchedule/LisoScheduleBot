using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Schedule;

public class ChooseScheduleHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;

    public ChooseScheduleHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public UserStep Step => UserStep.ChooseSchedule;

    public async Task Handle(Message message, User user)
    {
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.OpenBook} Обери розклад.",
            replyMarkup: KeyboardFactory.TodayFromDate()
        );
    }
}
