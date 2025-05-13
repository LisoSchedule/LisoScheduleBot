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
    private readonly IUserService _userService;

    public ChooseGroupHandler(IGroupService groupService, IMessageService messageService, IUserService userService)
    {
        _groupService = groupService;
        _messageService = messageService;
        _userService = userService;
    }

    public UserStep Step => UserStep.ChooseGroup;

    public async Task Handle(Message message, User user)
    {
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: "Обери свою групу.",
            replyMarkup: KeyboardFactory.GroupsList(await _groupService.GetUniqueGroups())
        );

        user.Step = UserStep.ChooseGroup;
        await _userService.SaveUser(user);
        //api request
    }
}
