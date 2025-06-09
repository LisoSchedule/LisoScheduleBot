using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Settings;

public class ChangeEmailHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;

    public ChangeEmailHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public UserStep Step => UserStep.ChangeEmail;

    public async Task Handle(Message message, User user)
    {
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: user.Email == string.Empty
                ? $"{Emoji.Pen} Бажаєш вказати Email?"
                : $"{Emoji.Email} Поточний Email: *{user.Email}*\n\n" +
                $"{Emoji.TrashCan} Бажаєш видалити Email?",
            replyMarkup: user.Email == string.Empty
                ? KeyboardFactory.YesLater("email")
                : KeyboardFactory.RemoveCancel("email"),
            parseMode: ParseMode.Markdown
        );
    }
}
