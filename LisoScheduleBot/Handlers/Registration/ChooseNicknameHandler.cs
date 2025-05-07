using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Registration;

public class ChooseNicknameHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;

    public ChooseNicknameHandler(IMessageService messageService, IUserService userService)
    {
        _messageService = messageService;
        _userService = userService;
    }

    public UserStep Step => UserStep.ChooseNickname;

    public async Task Handle(Message message, User user)
    {
        var tgUser = message.From;
        var firstName = tgUser!.FirstName;
        var username = tgUser.Username;

        await _messageService.SendMessage(
           chatId: user.ChatId,
           text: $"{Emoji.WavingHand} Привіт!",
           replyMarkup: KeyboardFactory.StartOver()
        );
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.Pencil} Бажаєш задати нікнейм?",
            replyMarkup: KeyboardFactory.YesLaterNicknames(firstName, username)
        );

        user.Step = UserStep.ChooseNickname;
        await _userService.SaveUser(user);
    }
}
