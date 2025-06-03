using Telegram.Bot;
using Telegram.Bot.Types;
using LisoScheduleBot.Config;

namespace LisoScheduleBot.Services;

public class BotService : IHostedService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<BotService> _logger;
    private readonly string _webhookUrl;

    public BotService(ITelegramBotClient botClient, AppConfig config, ILogger<BotService> logger)
    {
        _botClient = botClient;
        _logger = logger;
        _webhookUrl = config.WebhookUrl;
    }

    public async Task StartAsync(CancellationToken ct)
    {
        var me = await _botClient.GetMe(ct);
        _logger.LogInformation("{Username} has started working.", me.Username);

        await SetBotCommands();
        await SetBotWebhook();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _botClient.DeleteWebhook(cancellationToken: cancellationToken);
        _logger.LogInformation("Webhook deleted.");
    }

    private async Task SetBotCommands()
    {
        var commands = new[]
        {
            new BotCommand { Command = "start", Description = "старт" }
        };

        await _botClient.SetMyCommands(commands);
    }

    private async Task SetBotWebhook()
    {
        await _botClient.SetWebhook(_webhookUrl);
        _logger.LogInformation("Webhook set to {WebhookUrl}.", _webhookUrl);
    }
}
