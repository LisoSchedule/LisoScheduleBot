using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Settings;

public class ChangingNicknameHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;

    public ChangingNicknameHandler(IMessageService messageService, IUserService userService)
    {
        _messageService = messageService;
        _userService = userService;
    }

    public UserStep Step => UserStep.ChangingNickname;

    public async Task Handle(Message message, User user)
    {
        var nickname = message.Text;

        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.CheckMark} Чудово, нікнейм вказано.\n\n" +
                $"{Emoji.PhoneWithArrow} Обирай, що забажаєш.",
            replyMarkup: KeyboardFactory.Settings(user.Settings)
        );

        user.Nickname = nickname;
        user.Step = UserStep.ChangeSettings;
        await _userService.SaveUser(user);
    }
}
