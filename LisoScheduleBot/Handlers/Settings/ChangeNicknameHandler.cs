using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Settings;

public class ChangeNicknameHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;

    public ChangeNicknameHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public UserStep Step => UserStep.ChangeNickname;

    public async Task Handle(Message message, User user)
    {
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: user.Nickname != null 
                ? $"{Emoji.Pen} ЅажаЇш задати н≥кнейм?"
                : $"{Emoji.Pen} ЅажаЇш зм≥нити н≥кнейм?",
            replyMarkup: KeyboardFactory.YesLater()
        );
    }
}
