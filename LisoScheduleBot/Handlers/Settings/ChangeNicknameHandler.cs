using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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
            text: user.Nickname == string.Empty
                ? $"{Emoji.Pen} Бажаєш задати нікнейм?"
                : $"{Emoji.Silhoutte} Поточний нікнейм: *{user.Nickname}*\n\n" +
                $"{Emoji.Pen} Бажаєш змінити нікнейм?",
            replyMarkup: KeyboardFactory.YesLater("nickname"),
            parseMode: ParseMode.Markdown
        );
    }
}
