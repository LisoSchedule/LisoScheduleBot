using Telegram.Bot.Types.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Utils;

namespace LisoScheduleBot.Services;

public class JsonNotificationService : INotificationService
{
    private readonly IScheduleService _scheduleService;
    private readonly IUserService _userService;
    private readonly IMessageService _messageService;

    public JsonNotificationService(IScheduleService scheduleService, IUserService userService, IMessageService messageService)
    {
        _scheduleService = scheduleService;
        _userService = userService;
        _messageService = messageService;
    }

    public async Task SendReminders()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        var time = TimeOnly.FromDateTime(DateTime.Now);
        var users = await _userService.GetAllUsers();

        foreach (var user in users)
        {
            if (!user.Settings.ReceiveNotifications) continue;

            var scheduleItems = await _scheduleService.GetScheduleItemsByDate(date, user);

            var nextItem = scheduleItems.FirstOrDefault(item => item.StartTime > time);
            if (nextItem == null) continue;

            var timeBeforeClass = (int)user.Settings.TimeBeforeClassToNotify;
            var notifyTime = nextItem.StartTime.AddMinutes(-timeBeforeClass);

            if (Math.Abs((notifyTime.ToTimeSpan() - time.ToTimeSpan()).TotalSeconds) <= 60.0)
            {
                await _messageService.SendMessage(
                    chatId: user.ChatId, 
                    text: $"{Emoji.Clock} *Нагадування* про пару:\n\n" +
                    $"{Emoji.Books} *Тип*: {nextItem.SubjectType}\n" +
                    $"{Emoji.ClosedBook} *Предмет*: {nextItem.Subject}\n" +
                    $"{Emoji.Silhoutte} *Викладач*: {nextItem.Teacher}\n" +
                    $"{Emoji.Pin} *Місце*: {nextItem.Classroom}\n" +
                    $"{Emoji.TwelveOClock} *Початок*: {nextItem.StartTime:HH:mm}\n" +
                    $"{Emoji.ThreeOClock} *Кінець*: {nextItem.StartTime.AddMinutes(nextItem.Duration):HH:mm}\n" +
                    $"{Emoji.Hourglass} *До початку*: {timeBeforeClass} хвилин",
                    parseMode: ParseMode.Markdown
                );
            }
        }
    }

}
