using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Registration;

public class ChoosingNicknameHandler : IUserStepHandler
{
    private readonly IGroupService _groupService;
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;

    public ChoosingNicknameHandler(IGroupService groupService, IMessageService messageService, IUserService userService)
    {
        _groupService = groupService;
        _messageService = messageService;
        _userService = userService;
    }

    public UserStep Step => UserStep.ChoosingNickname;

    public async Task Handle(Message message, User user)
    {
        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.CheckMark} Чудово, нікнейм вказано.\n" +
                $"{Emoji.Gear} Ти зможеш змінити його у налаштуваннях.\n\n" +
                $"{Emoji.Silhoutte} Обери свою групу.",
            replyMarkup: KeyboardFactory.GroupsList(await _groupService.GetUniqueGroups(), "registration")
        );

        var nickname = message.Text;
        user.Nickname = nickname;
        user.Step = UserStep.ChooseGroup;
        await _userService.SaveUser(user);
    }
}
