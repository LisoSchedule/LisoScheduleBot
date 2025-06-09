using Telegram.Bot.Types.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;

namespace LisoScheduleBot.Services;

public class JsonNotificationService : INotificationService
{
    private readonly IScheduleService _scheduleService;
    private readonly IUserService _userService;
    private readonly IMessageService _messageService;
    private readonly IEmailService _emailService;

    public JsonNotificationService(
        IScheduleService scheduleService, 
        IUserService userService, 
        IMessageService messageService,
        IEmailService emailService)
    {
        _scheduleService = scheduleService;
        _userService = userService;
        _messageService = messageService;
        _emailService = emailService;
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
                var text = $"{Emoji.Clock} <strong>Нагадування</strong> про пару:\n\n" +
                    $"{Emoji.Books} <strong>Тип</strong>: {nextItem.SubjectType}\n" +
                    $"{Emoji.ClosedBook} <strong>Предмет</strong>: {nextItem.Subject}\n" +
                    $"{Emoji.Silhoutte} <strong>Викладач</strong>: {nextItem.Teacher}\n" +
                    $"{Emoji.Pin} <strong>Місце</strong>: {nextItem.Classroom}\n" +
                    $"{Emoji.TwelveOClock} <strong>Початок</strong>: {nextItem.StartTime:HH:mm}\n" +
                    $"{Emoji.ThreeOClock} <strong>Кінець</strong>: " +
                    $"{nextItem.StartTime.AddMinutes(nextItem.Duration):HH:mm}\n" +
                    $"{Emoji.Hourglass} <strong>До початку</strong>: {timeBeforeClass} хвилин";

                await _messageService.SendMessage(
                    chatId: user.ChatId, 
                    text: text,
                    replyMarkup: KeyboardFactory.OpenSettings(),
                    parseMode: ParseMode.Html
                );

                await _emailService.SendMessage(
                    email: user.Email!,
                    subject: "Нагадування про пару",
                    body: text
                );
            }
        }
    }

}
