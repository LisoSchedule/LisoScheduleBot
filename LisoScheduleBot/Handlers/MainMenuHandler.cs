using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Schedule;

public class MainMenuHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;

    public MainMenuHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public UserStep Step => UserStep.MainMenu;

    public async Task Handle(Message message, User user)
    {
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.PhoneWithArrow} ќбери потр≥бну д≥ю.",
            replyMarkup: KeyboardFactory.MainMenu()
        );
    }
}
