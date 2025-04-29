using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Registration;

public class ChangeSettingsHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;

    public ChangeSettingsHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public UserStep Step => UserStep.ChangeSettings;

    public async Task Handle(Message message, User user)
    {
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: "Îבטנאי, שמ חאבאזא÷ר.",
            replyMarkup: KeyboardFactory.Settings(user.Settings)
        );
    }
}
