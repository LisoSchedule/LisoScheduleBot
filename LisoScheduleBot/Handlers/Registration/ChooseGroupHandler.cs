using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Registration;

public class ChooseGroupHandler : IUserStepHandler
{
    private readonly IGroupService _groupService;
    private readonly IMessageService _messageService;

    public ChooseGroupHandler(IGroupService groupService, IMessageService messageService)
    {
        _groupService = groupService;
        _messageService = messageService;
    }

    public UserStep Step => UserStep.ChooseGroup;

    public async Task Handle(Message message, User user)
    {
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.Silhoutte} Обери свою групу.",
            replyMarkup: KeyboardFactory.GroupsList(await _groupService.GetUniqueGroups(), "registration")
        );
    }
}
