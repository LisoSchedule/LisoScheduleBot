using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Registration;

public class ChooseSubGroupHandler : IUserStepHandler
{
    private readonly IGroupService _groupService;
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;

    public ChooseSubGroupHandler(IGroupService groupService, IMessageService messageService, IUserService userService)
    {
        _groupService = groupService;
        _messageService = messageService;
        _userService = userService;
    }

    public UserStep Step => UserStep.ChooseSubGroup;

    public async Task Handle(Message message, User user)
    {
        var group = await _groupService.GetGroup(user.GroupId);

        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.DoubleSilhoutte} Обери свою підгрупу.",
            replyMarkup: KeyboardFactory.SubGroupsList(await _groupService.GetGroups(group.Name!.ToString()))
        );

        user.Step = UserStep.ChooseSubGroup;
        await _userService.SaveUser(user);
    }
}
