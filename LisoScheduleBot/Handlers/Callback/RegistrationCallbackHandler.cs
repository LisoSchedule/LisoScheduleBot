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

    public RegistrationCallbackHandler(IGroupService groupService, IMessageService messageService, 
        IUserService userService)
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

        if (user.Step >= UserStep.MainMenu)
        {
            await _messageService.DeleteMessage(chatId, messageId);

            await _messageService.SendMessage(
                chatId: user.ChatId,
                text: $"{Emoji.Warning} Тебе вже зареєстровано.\n\n" +
                    $"{Emoji.PhoneWithArrow} Обери потрібну дію.",
                replyMarkup: KeyboardFactory.MainMenu()
            );

            return;
        }

        switch (message)
        {
            case "yes":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.WritingHand} Введи бажаний нікнейм."
                );

                user.Step = UserStep.ChoosingNickname;
                await _userService.SaveUser(user);
                break;

            case "later":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.Silhoutte} Обери свою групу.",
                    replyMarkup: KeyboardFactory.GroupsList(await _groupService.GetUniqueGroups(), "registration")
                );

                user.Step = UserStep.ChooseGroup;
                await _userService.SaveUser(user);
                break;

            case "nickname":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.CheckMark} Чудово, нікнейм задано.\n" +
                        $"{Emoji.Gear} Ти зможеш змінити його у налаштуваннях.\n\n" +
                        $"{Emoji.Silhoutte} Обери свою групу.",
                    replyMarkup: KeyboardFactory.GroupsList(await _groupService.GetUniqueGroups(), "registration")
                );

                var nickname = callbackData[2];
                user.Nickname = nickname;
                user.Step = UserStep.ChooseGroup;
                await _userService.SaveUser(user);
                break;

            case "group_name":
                var groupName = callbackData[2];

                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.DoubleSilhoutte} Обери свою підгрупу.",
                    replyMarkup: KeyboardFactory.SubGroupsList(await _groupService.GetGroups(groupName), "registration")
                );

                var group = await _groupService.GetGroup(groupName);
                user.GroupId = group.GroupId;
                user.Step = UserStep.ChooseSubGroup;
                await _userService.SaveUser(user);
                break;

            case "group_id":
                var groupId = int.Parse(callbackData[2]);

                await _messageService.DeleteMessage(chatId, messageId);

                await _messageService.SendMessage(
                    chatId: user.ChatId,
                    text: $"{Emoji.RacingFlag} Тебе успішно зареєстровано!\n\n" +
                    $"{Emoji.PhoneWithArrow} Обери потрібну дію.",
                    replyMarkup: KeyboardFactory.MainMenu()
                );

                user.Step = UserStep.MainMenu;
                user.GroupId = groupId;
                await _userService.SaveUser(user);
                break;
        }
    }
}
