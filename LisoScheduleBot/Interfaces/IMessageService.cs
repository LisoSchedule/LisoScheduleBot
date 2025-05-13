using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace LisoScheduleBot.Interfaces;

public interface IMessageService
{
    Task SendMessage(long chatId, string text, ReplyMarkup? replyMarkup = null, ParseMode parseMode = ParseMode.None);
    Task EditMessage(long chatId, int messageId, string text, InlineKeyboardMarkup? replyMarkup = null, ParseMode parseMode = ParseMode.None);
    Task DeleteMessage(long chatId, int messageId);
}
