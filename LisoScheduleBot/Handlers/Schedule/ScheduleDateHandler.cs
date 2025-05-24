using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Schedule;

public class ScheduleDateHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;
    private readonly IScheduleService _scheduleService;

    public ScheduleDateHandler(IMessageService messageService, IScheduleService scheduleService)
    {
        _messageService = messageService;
        _scheduleService = scheduleService;
    }

    public UserStep Step => UserStep.ScheduleDate;
    public async Task Handle(Message message, User user)
    {
        var scheduleItems = await _scheduleService.GetScheduleItemsByDate(DateOnly.FromDateTime(DateTime.UtcNow), user);
        var schedule = string.Empty;

        foreach (var item in scheduleItems)
        {
            schedule += $"*Тип*: {item.SubjectType}\n" +
                $"*Предмет*: {item.Subject}\n" +
                $"*Викладач*: {item.Teacher}\n" +
                $"*Місце*: {item.Classroom}\n" +
                $"*Початок*: {item.StartTime:HH:mm}\n" +
                $"*Кінець*: {item.StartTime.AddMinutes(item.Duration):HH:mm}\n\n";
        }

        var text = $"{Emoji.Date} *Розклад* на {DateTime.UtcNow:dd.MM.yy}";
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
