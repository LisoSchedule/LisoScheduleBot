using LisoScheduleBot.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace LisoScheduleBot.Utils;

public static class KeyboardFactory
{
    public static ReplyKeyboardMarkup StartOver()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "Почати заново" }
        })
        {
            ResizeKeyboard = true
        };
    }

    public static InlineKeyboardMarkup YesLaterNickname(string firstName, string? username = null)
    {
        var buttons = new List<InlineKeyboardButton[]>
        {
            new[]
            {
                InlineButton("Так", "registration:yes"),
                InlineButton("Пізніше", "registration:later")
            },
            new[]
            {
                InlineButton($"{firstName}", $"registration:nickname:{firstName}")
            }
        };

        if (!string.IsNullOrEmpty(username))
        {
            buttons.Add(new[]
            {
                InlineButton($"{username}", $"registration:nickname:{firstName}")
            });
        }

        return new InlineKeyboardMarkup(buttons);
    }

    public static InlineKeyboardMarkup GroupsList(List<Group> groups)
    {
        var buttons = groups
            .Select(group => InlineButton(group.Name!, $"registration:group_name:{group.Name}"))
            .ToArray();

        return new InlineKeyboardMarkup(buttons);
    }

    public static InlineKeyboardMarkup SubGroupsList(List<Group> groups)
    {
        var buttons = groups
            .Select(group => InlineButton(group.SubGroup.ToString(), $"registration:group_id:{group.GroupId}"))
            .ToArray();

        return new InlineKeyboardMarkup(buttons);
    }

    public static ReplyKeyboardMarkup MainMenu()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "Розклад", "Налаштування" }
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };
    }

    private static InlineKeyboardButton InlineButton(string text, string callbackData)
    {
        return InlineKeyboardButton.WithCallbackData(text, callbackData);
    }
}
