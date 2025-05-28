using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Callback;

public class SettingsCallbackHandler : ICallbackHandler
{
    private readonly IGroupService _groupService;
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;

    public SettingsCallbackHandler(IGroupService groupService, IMessageService messageService, IUserService userService)
    {
        _groupService = groupService;
        _messageService = messageService;
        _userService = userService;
    }

    public bool CanHandle(string callbackData) => callbackData.StartsWith("settings:");

    public async Task Handle(CallbackQuery callbackQuery, User user)
    {
        var callbackData = callbackQuery.Data!.Split(':');
        var message = callbackData[1];
        var messageId = callbackQuery.Message!.MessageId;
        var chatId = user.ChatId;
        Group group;

        switch (message)
        {
            case "nickname":
                await _messageService.EditMessage(
                    chatId: user.ChatId,
                    messageId: messageId,
                    text: user.Nickname == string.Empty
                        ? $"{Emoji.Pen} Бажаєш задати нікнейм?"
                        : $"{Emoji.Silhoutte} Поточний нікнейм: *{user.Nickname}*\n\n" +
                        $"{Emoji.Pen} Бажаєш змінити нікнейм?",
                    replyMarkup: KeyboardFactory.YesLater("nickname"),
                    parseMode: ParseMode.Markdown
                );

                user.Step = UserStep.ChangeNickname;
                await _userService.SaveUser(user);
                break;

            case "nickname_yes":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.WritingHand} Введи бажаний нікнейм."
                );

                user.Step = UserStep.ChangingNickname;
                await _userService.SaveUser(user);
                break;

            case "nickname_later":
            case "group_later":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.PhoneWithArrow} Обирай, що забажаєш.",
                    replyMarkup: KeyboardFactory.Settings(user.Settings)
                );

                user.Step = UserStep.ChangeSettings;
                await _userService.SaveUser(user);
                break;

            case "group":
                group = await _groupService.GetGroup(user.GroupId);

                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.Silhoutte} Поточна група: *{EnumConverter<GroupName>.EnumToString(group.Name)}/{group.SubGroup}*\n\n" +
                        $"{Emoji.Refresh} Бажаєш змінити групу?",
                    replyMarkup: KeyboardFactory.YesLater("group"),
                    parseMode: ParseMode.Markdown
                );

                user.Step = UserStep.ChangeGroup;
                await _userService.SaveUser(user);
                break;

            case "group_yes":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.Silhoutte} Обери свою групу.",
                    replyMarkup: KeyboardFactory.GroupsList(await _groupService.GetUniqueGroups(), "settings")
                );

                user.Step = UserStep.ChangingGroup;
                await _userService.SaveUser(user);
                break;

            case "group_name":
                var groupName = callbackData[2];

                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.DoubleSilhoutte} Обери свою підгрупу.",
                    replyMarkup: KeyboardFactory.SubGroupsList(await _groupService.GetGroups(groupName), "settings")
                );

                group = await _groupService.GetGroup(groupName);
                user.GroupId = group.GroupId;
                user.Step = UserStep.ChangingSubGroup;
                await _userService.SaveUser(user);
                break;

            case "group_id":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.CheckMark} Чудово, групу змінено.\n\n" +
                        $"{Emoji.PhoneWithArrow} Обирай, що забажаєш.",
                    replyMarkup: KeyboardFactory.Settings(user.Settings)
                );

                var groupId = int.Parse(callbackData[2]);
                user.GroupId = groupId;
                user.Step = UserStep.MainMenu;
                await _userService.SaveUser(user);
                break;

            case "notifications":
                user.Settings.ReceiveNotifications = !user.Settings.ReceiveNotifications;

                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.PhoneWithArrow} Обирай, що забажаєш.",
                    replyMarkup: KeyboardFactory.Settings(user.Settings)
                );

                await _userService.SaveUser(user);
                break;

            case "time_before_class":
                var values = Enum.GetValues<TimeBeforeClass>();
                var index = Array.IndexOf(values, user.Settings.TimeBeforeClassToNotify);
                user.Settings.TimeBeforeClassToNotify = values[(index + 1) % values.Length];

                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.PhoneWithArrow} Обирай, що забажаєш.",
                    replyMarkup: KeyboardFactory.Settings(user.Settings)
                );

                await _userService.SaveUser(user);
                break;

            case "profile":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.PersonWithTrash} Бажаєш видалити профіль?",
                    replyMarkup: KeyboardFactory.RemoveCancel()
                );

                user.Step = UserStep.RemoveProfile;
                await _userService.SaveUser(user);
                break;

            case "remove":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.CheckMark} Профіль успішно видалено."
                );

                await _userService.RemoveUser(user);
                break;

            case "cancel":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.PhoneWithArrow} Обирай, що забажаєш.",
                    replyMarkup: KeyboardFactory.Settings(user.Settings)
                );
                
                user.Step = UserStep.ChangeSettings;
                await _userService.SaveUser(user);
                break;

            case "main_menu":
                await _messageService.DeleteMessage(chatId, messageId);
                await _messageService.SendMessage(
                    chatId: chatId,
                    text: $"{Emoji.PhoneWithArrow} Обери потрібну дію.",
                    replyMarkup: KeyboardFactory.MainMenu()
                );

                user.Step = UserStep.MainMenu;
                await _userService.SaveUser(user);
                break;
        }
    }
}
