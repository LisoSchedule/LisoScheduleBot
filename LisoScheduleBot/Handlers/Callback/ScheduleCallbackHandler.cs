using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Callback;

public class ScheduleCallbackHandler : ICallbackHandler
{
    private readonly IMessageService _messageService;
    private readonly IScheduleService _scheduleService;
    private readonly IUserService _userService;

    public ScheduleCallbackHandler(IMessageService messageService, IScheduleService scheduleService,
        IUserService userService)
    {
        _messageService = messageService;
        _scheduleService = scheduleService;
        _userService = userService;
    }

    public bool CanHandle(string callbackData) => callbackData.StartsWith("schedule:");

    public async Task Handle(CallbackQuery callbackQuery, User user)
    {
        var callbackData = callbackQuery.Data!.Split(':');
        var message = callbackData[1];
        var messageId = callbackQuery.Message!.MessageId;
        var chatId = user.ChatId;

        List<ScheduleItem>? scheduleItems;
        string schedule = string.Empty;

        switch (message)
        {
            case "today":
                scheduleItems = await _scheduleService.GetScheduleItemsByDate(DateOnly.FromDateTime(DateTime.UtcNow), user);

                foreach (var item in scheduleItems)
                {
                    schedule += $"Предмет: {item.Subject}\n" +
                        $"Викладач: {item.Teacher}\n" +
                        $"Місце: {item.Classroom}\n" +
                        $"Початок: {item.StartTime:HH:mm}\n" +
                        $"Кінець: {item.StartTime.AddMinutes(item.Duration):HH:mm}\n\n";
                }

                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.Date} Розклад на сьогодні ({DateTime.UtcNow:dd.MM.yy}):\n\n" + schedule,
                    replyMarkup: KeyboardFactory.TodayBack()
                );

                user.Step = UserStep.ScheduleToday;
                await _userService.SaveUser(user);
                break;

            case "from_date":
            case "this_week":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.Calendar} Обери день.",
                    replyMarkup: KeyboardFactory.WeekButtons()
                );

                user.Step = UserStep.ScheduleThisWeek;
                await _userService.SaveUser(user);
                break;

            case "date":
                var date = callbackData[2];
                scheduleItems = await _scheduleService.GetScheduleItemsByDate(DateOnly.Parse(date), user);

                foreach (var item in scheduleItems)
                {
                    schedule += $"Предмет: {item.Subject}\n" +
                        $"Викладач: {item.Teacher}\n" +
                        $"Місце: {item.Classroom}\n" +
                        $"Початок: {item.StartTime:HH:mm}\n" +
                        $"Кінець: {item.StartTime.AddMinutes(item.Duration):HH:mm}\n\n";
                }

                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.Date} Розклад на {date}:\n\n" + schedule,
                    replyMarkup: KeyboardFactory.DateBack(DateOnly.Parse(date))
                );

                user.Step = UserStep.ScheduleDate;
                await _userService.SaveUser(user);
                break;

            case "next":
            case "next_week":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.Calendar} Обери день.",
                    replyMarkup: KeyboardFactory.NextWeekButtons()
                );

                user.Step = UserStep.ScheduleNextWeek;
                await _userService.SaveUser(user);
                break;

            case "previous":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.Calendar} Обери день.",
                    replyMarkup: KeyboardFactory.WeekButtons()
                );

                user.Step = UserStep.ScheduleThisWeek;
                await _userService.SaveUser(user);
                break;

            case "back":
                await _messageService.EditMessage(
                    chatId: chatId,
                    messageId: messageId,
                    text: $"{Emoji.OpenBook} Обери розклад.",
                    replyMarkup: KeyboardFactory.TodayFromDate()
                );

                user.Step = UserStep.ChooseSchedule;
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