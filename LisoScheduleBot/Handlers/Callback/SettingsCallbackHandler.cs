using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Callback;

public class SettingsCallbackHandler : ICallbackHandler
{
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;

    public SettingsCallbackHandler(IMessageService messageService, IUserService userService)
    {
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

        switch (message)
        {
            case "nickname":
                await _messageService.EditMessage(
                    chatId: user.ChatId,
                    messageId: messageId,
                    text: user.Nickname == string.Empty ? "Бажаєш задати нікнейм?" : "Бажаєш змінити нікнейм?",
                    replyMarkup: KeyboardFactory.YesLater()
                );

                user.Step = UserStep.ChangeNickname;
                await _userService.SaveUser(user);
                break;

            case "yes":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Введи бажаний нікнейм."
                );

                user.Step = UserStep.ChangingNickname;
                await _userService.SaveUser(user);
                break;

            case "later":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Обирай, що забажаєш.",
                    replyMarkup: KeyboardFactory.Settings(user.Settings)
                );

                user.Step = UserStep.ChangeSettings;
                await _userService.SaveUser(user);
                break;

            case "notifications":
                user.Settings.ReceiveNotifications = !user.Settings.ReceiveNotifications;

                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Обирай, що забажаєш.",
                    replyMarkup: KeyboardFactory.Settings(user.Settings)
                );

                await _userService.SaveUser(user);
                break;

            case "time_before_class":
                switch (user.Settings.TimeBeforeClassToNotify)
                {
                    case TimeBeforeClass.FifteenMinutes:
                        user.Settings.TimeBeforeClassToNotify = TimeBeforeClass.ThirtyMinutes;
                        break;

                    case TimeBeforeClass.ThirtyMinutes:
                        user.Settings.TimeBeforeClassToNotify = TimeBeforeClass.OneHour;
                        break;

                    case TimeBeforeClass.OneHour:
                        user.Settings.TimeBeforeClassToNotify = TimeBeforeClass.FifteenMinutes;
                        break;
                }

                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Обирай, що забажаєш.",
                    replyMarkup: KeyboardFactory.Settings(user.Settings)
                );

                await _userService.SaveUser(user);
                break;

            case "profile":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Бажаєш видалити профіль?",
                    replyMarkup: KeyboardFactory.RemoveCancel()
                );

                user.Step = UserStep.RemoveProfile;
                await _userService.SaveUser(user);
                break;

            case "remove":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Профіль успішно видалено."
                );

                await _userService.RemoveUser(user);
                break;

            case "cancel":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: "Обирай, що забажаєш.",
                    replyMarkup: KeyboardFactory.Settings(user.Settings)
                );
                
                user.Step = UserStep.ChangeSettings;
                await _userService.SaveUser(user);
                break;

            case "main_menu":
                await _messageService.DeleteMessage(chatId, messageId);
                await _messageService.SendMessage(
                    chatId: chatId,
                    text: "Обери потрібну дію.",
                    replyMarkup: KeyboardFactory.MainMenu()
                );

                user.Step = UserStep.MainMenu;
                await _userService.SaveUser(user);
                break;
        }
    }
}
