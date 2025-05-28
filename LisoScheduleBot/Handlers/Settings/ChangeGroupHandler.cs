using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Settings;

public class ChangeGroupHandler : IUserStepHandler
{
    private readonly IGroupService _groupService;
    private readonly IMessageService _messageService;

    public ChangeGroupHandler(IGroupService groupService, IMessageService messageService)
    {
        _groupService = groupService;
        _messageService = messageService;
    }

    public UserStep Step => UserStep.ChangeGroup;

    public async Task Handle(Message message, User user)
    {
        var group = await _groupService.GetGroup(user.GroupId);

        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.Silhoutte} Поточна група: *{EnumConverter<GroupName>.EnumToString(group.Name)}/{group.SubGroup}*\n\n" +
                $"{Emoji.Refresh} Бажаєш змінити групу?",
            replyMarkup: KeyboardFactory.YesLater("group"),
            parseMode: ParseMode.Markdown
        );
    }
}
