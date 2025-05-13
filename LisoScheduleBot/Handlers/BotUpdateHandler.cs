using Telegram.Bot;
using Telegram.Bot.Types;

namespace LisoScheduleBot.Handlers;

public class BotUpdateHandler
{
    internal async Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    internal async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
