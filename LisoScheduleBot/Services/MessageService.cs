using Telegram.Bot;
using LisoScheduleBot.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace LisoScheduleBot.Services;

public class MessageService : IMessageService
{
    private readonly ITelegramBotClient _botClient;
    public MessageService(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task SendMessage(long chatId, string text, ReplyMarkup? replyMarkup = null, ParseMode parseMode = ParseMode.None)
    {
        await _botClient.SendMessage(
            chatId: chatId,
            text: text,
            replyMarkup: replyMarkup,
            parseMode: parseMode
        );
    }

    public async Task EditMessage(long chatId, int messageId, string text, InlineKeyboardMarkup? replyMarkup = null, ParseMode parseMode = ParseMode.None)
    {
        await _botClient.EditMessageText(
            chatId: chatId,
            messageId: messageId,
            text: text,
            replyMarkup: replyMarkup,
            parseMode: parseMode
        );
    }

    public async Task DeleteMessage(long chatId, int messageId)
    {
        await _botClient.DeleteMessage(
            chatId: chatId,
            messageId: messageId
        );
    }
}
