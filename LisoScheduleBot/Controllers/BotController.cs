using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using LisoScheduleBot.Handlers;
using Telegram.Bot;

namespace LisoScheduleBot.Controllers;

[ApiController]
[Route("api/bot")]
public class BotController : ControllerBase
{
    private readonly ITelegramBotClient _botClient;
    private readonly BotUpdateHandler _updateHandler;

    public BotController(ITelegramBotClient botClient, BotUpdateHandler updateHandler)
    {
        _botClient = botClient;
        _updateHandler = updateHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        await _updateHandler.HandleUpdate(_botClient, update, default);
        return Ok();
    }
}
