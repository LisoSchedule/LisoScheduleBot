using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Callback;

public class RegistrationCallbackHandler : ICallbackHandler
{
    private readonly IGroupService _groupService;
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;

    public RegistrationCallbackHandler(IGroupService groupService, IMessageService messageService, IUserService userService)
    {
        _groupService = groupService;
        _messageService = messageService;
        _userService = userService;
    }

    public bool CanHandle(string callbackData) => callbackData.StartsWith("registration:");

    public async Task Handle(CallbackQuery callbackQuery, User user)
    {
        var callbackData = callbackQuery.Data!.Split(':');
        var message = callbackData[1];
        var messageId = callbackQuery.Message!.MessageId;
        var chatId = user.ChatId;

        switch (message)
        {
            case "yes":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Введи бажаний нікнейм."
                );

                user.Step = UserStep.ChoosingNickname;
                await _userService.SaveUser(user);
                break;

            case "later":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Обери свою групу.",
                    replyMarkup: KeyboardFactory.GroupsList(await _groupService.GetUniqueGroups())
                );

                user.Step = UserStep.ChooseGroup;
                await _userService.SaveUser(user);
                break;

            case "nickname":
                var nickname = callbackData[2];

                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Чудово, нікнейм задано. Ти зможеш змінити його у налаштуваннях." +
                        "\n\nОбери свою групу.",
                    replyMarkup: KeyboardFactory.GroupsList(await _groupService.GetUniqueGroups())
                );

                user.Nickname = nickname;
                user.Step = UserStep.ChooseGroup;
                await _userService.SaveUser(user);
                break;

            case "group_name":
                var groupName = callbackData[2];
                var group = await _groupService.GetGroup(groupName);

                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Обери свою підгрупу.",
                    replyMarkup: KeyboardFactory.SubGroupsList(await _groupService.GetGroups(groupName))
                );

                user.GroupId = group.GroupId;
                user.Step = UserStep.ChooseSubGroup;
                await _userService.SaveUser(user);
                break;

            case "group_id":
                var groupId = int.Parse(callbackData[2]);

                await _messageService.DeleteMessage(chatId, messageId);
                await _messageService.SendMessage(
                    chatId: user.ChatId,
                    text: "Тебе успішно зареєстровано!",
                    replyMarkup: KeyboardFactory.MainMenu()
                );

                user.Step = UserStep.MainMenu;
                user.GroupId = groupId;
                await _userService.SaveUser(user);
                break;
        }
    }
}
