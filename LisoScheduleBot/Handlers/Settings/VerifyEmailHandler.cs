using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Settings;

public class VerifyEmailHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;

    public VerifyEmailHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public UserStep Step => UserStep.VerifyEmail;

    public async Task Handle(Message message, User user)
    {
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.LockWithKey} Верифікаційний код відправлено на введений Email.",
            replyMarkup: KeyboardFactory.InputCancel()
        );
    }
}
