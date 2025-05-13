using Telegram.Bot;
using LisoScheduleBot.Config;
using LisoScheduleBot.Handlers;

namespace LisoScheduleBot.Services;

public class BotService : BackgroundService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<BotService> _logger;
    private readonly BotUpdateHandler _updateHandler;

    public BotService(AppConfig config, BotUpdateHandler updateHandler, ILogger<BotService> logger)
    {
        _botClient = new TelegramBotClient(config.BotToken);
        _updateHandler = updateHandler;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var me = await _botClient.GetMe(ct);
        _logger.LogInformation("{Username} has started working.", me.Username);

        await _botClient.ReceiveAsync(
            updateHandler: _updateHandler.HandleUpdateAsync,
            errorHandler: _updateHandler.HandleErrorAsync,
            cancellationToken: ct
        );
    }
}
