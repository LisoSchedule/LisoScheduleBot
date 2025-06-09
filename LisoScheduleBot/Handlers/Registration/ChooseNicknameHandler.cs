using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Registration;

public class ChooseNicknameHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;

    public ChooseNicknameHandler(IMessageService messageService)
    {
        _messageService = messageService;
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
            text: $"{Emoji.Pen} Бажаєш вказати нікнейм?",
            replyMarkup: KeyboardFactory.YesLaterNicknames(firstName, username)
        );
    }
}
