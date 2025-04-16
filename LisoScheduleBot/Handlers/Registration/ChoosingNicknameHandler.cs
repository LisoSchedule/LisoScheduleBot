using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Registration;

public class ChoosingNicknameHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;

    public ChoosingNicknameHandler(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public UserStep Step => UserStep.ChoosingNickname;

    public async Task Handle(Message message, User user)
    {
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: "Чудово, нікнейм задано. Ти зможеш змінити його у налаштуваннях." +
            "\n\nОбери свою групу.",
            replyMarkup: KeyboardFactory.GroupsList(new List<Group>())
        );

        //http запит
    }
}
