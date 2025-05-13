using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Settings;

public class RemoveProfileHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;

    public RemoveProfileHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public UserStep Step => UserStep.RemoveProfile;

    public async Task Handle(Message message, User user)
    {
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.PersonWithTrash} Бажаєш видалити профіль?",
            replyMarkup: KeyboardFactory.RemoveCancel()
        );
    }
}
