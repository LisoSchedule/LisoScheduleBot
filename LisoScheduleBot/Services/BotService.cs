using Telegram.Bot;
using Telegram.Bot.Types;
using LisoScheduleBot.Handlers;

namespace LisoScheduleBot.Services;

public class BotService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly BotUpdateHandler _updateHandler;
    private readonly ILogger<BotService> _logger;

    public BotService(ITelegramBotClient botClient, BotUpdateHandler updateHandler, ILogger<BotService> logger)
    {
        _botClient = botClient;
        _updateHandler = updateHandler;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var me = await _botClient.GetMe(ct);
        Console.Clear();
        _logger.LogInformation("{Username} has started working.", me.Username);

        await SetBotCommands();

        await _botClient.ReceiveAsync(
            updateHandler: _updateHandler.HandleUpdate,
            errorHandler: _updateHandler.HandleError,
            cancellationToken: ct
        );
    }

    private async Task SetBotCommands()
    {
        var commands = new[]
        {
            new BotCommand { Command = "start", Description = "старт" }
        };

        await _botClient.SetMyCommands(commands);
    }
}
