using Telegram.Bot;
using Telegram.Bot.Types;
using LisoScheduleBot.Dispatchers;
using LisoScheduleBot.Interfaces;

namespace LisoScheduleBot.Handlers;

public class BotUpdateHandler
{
    private readonly UserStepDispatcher _userStepDispatcher;
    private readonly CallbackDispatcher _callbackDispatcher;
    private readonly MessageHandler _messageHandler;
    private readonly IUserService _userService;
    private readonly ILogger<BotUpdateHandler> _logger;

    public BotUpdateHandler(UserStepDispatcher userStepDispatcher, CallbackDispatcher callbackDispatcher, 
                            MessageHandler messageHandler, IUserService userService, ILogger<BotUpdateHandler> logger)
    {
        _userStepDispatcher = userStepDispatcher;
        _callbackDispatcher = callbackDispatcher;
        _messageHandler = messageHandler;
        _userService = userService;
        _logger = logger;
    }

    internal async Task HandleUpdate(ITelegramBotClient client, Update update, CancellationToken ct)
    {
        await (update switch
        {
            { Message: { } message } => OnMessage(client, message, ct),
            { CallbackQuery: { } callbackQuery } => OnCallbackQuery(client, callbackQuery, ct),
            _ => OnUnknownUpdate(client, update, ct)
        });
    }

    internal Task HandleError(ITelegramBotClient client, Exception exception, CancellationToken ct)
    {
        _logger.LogError("An error occurred: {exceptionMessage}", exception.Message);
        return Task.CompletedTask;
    }

    private async Task OnMessage(ITelegramBotClient client, Message message, CancellationToken ct)
    {
        if (message.Text is null) return;

        var username = message.From!.Username ?? message.From.FirstName;
        var user = await _userService.GetOrCreateUser(message.From!.Id, username);

        await _messageHandler.Handle(message, user);
        await _userStepDispatcher.Dispatch(message, user);
    }

    private async Task OnCallbackQuery(ITelegramBotClient client, CallbackQuery callbackQuery, CancellationToken ct)
    {
        if (callbackQuery.Data is null) return;

        var username = callbackQuery.From.Username ?? callbackQuery.From.FirstName;
        var user = await _userService.GetOrCreateUser(callbackQuery.From.Id, username);

        await _callbackDispatcher.Dispatch(callbackQuery, user);
    }

    private async Task OnUnknownUpdate(ITelegramBotClient client, Update update, CancellationToken ct)
    {
        _logger.LogWarning("Unknown update type: {updateType}", update.Type);
        await Task.CompletedTask;
    }
}
