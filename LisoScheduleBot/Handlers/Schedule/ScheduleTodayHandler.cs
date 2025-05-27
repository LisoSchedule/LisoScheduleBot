using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Schedule;

public class ScheduleTodayHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;
    private readonly IScheduleService _scheduleService;

    public ScheduleTodayHandler(IMessageService messageService, IScheduleService scheduleService)
    {
        _messageService = messageService;
        _scheduleService = scheduleService;
    }

    public UserStep Step => UserStep.ScheduleToday;

    public async Task Handle(Message message, User user)
    {
        var scheduleItems = await _scheduleService.GetScheduleItemsByDate(DateOnly.FromDateTime(DateTime.UtcNow), user);
        var schedule = string.Empty;

        foreach (var item in scheduleItems)
        {
            schedule += $"{Emoji.Books} *Тип*: {item.SubjectType}\n" +
                $"{Emoji.ClosedBook} *Предмет*: {item.Subject}\n" +
                $"{Emoji.Silhoutte} *Викладач*: {item.Teacher}\n" +
                $"{Emoji.Pin} *Місце*: {item.Classroom}\n" +
                $"{Emoji.TwelveOClock} *Початок*: {item.StartTime:HH:mm}\n" +
                $"{Emoji.ThreeOClock} *Кінець*: {item.StartTime.AddMinutes(item.Duration):HH:mm}\n\n";
        }

        var text = $"{Emoji.Date} *Розклад* на сьогодні ({DateTime.UtcNow:dd.MM.yy})";
        text += scheduleItems.Count == 0
            ? " відсутній."
            : ":\n\n" + schedule;

        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: text,
            replyMarkup: KeyboardFactory.TodayBack(),
            parseMode: ParseMode.Markdown
        );
    }
}
