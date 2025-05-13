using LisoScheduleBot.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace LisoScheduleBot.Utils;

public static class KeyboardFactory
{
    public static ReplyKeyboardMarkup StartOver()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { "ÐÐ¾ÑÐ°ÑÐ¸ Ð·Ð°Ð½Ð¾Ð²Ð¾" }
        })
        {
            ResizeKeyboard = true
        };
    }

    public static InlineKeyboardMarkup YesLaterNicknames(string firstName, string? username = null)
    {
        var buttons = new List<InlineKeyboardButton[]>
        {
            new[]
            {
                InlineButton("Ð¢Ð°Ðº", "registration:yes"),
                InlineButton("ÐÑÐ·Ð½ÑÑÐµ", "registration:later")
            },
            new[] { InlineButton($"{firstName}", $"registration:nickname:{firstName}") }
        };

        if (!string.IsNullOrEmpty(username))
        {
            buttons.Add(new[] { InlineButton($"{username}", $"registration:nickname:{username}") });
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
            new KeyboardButton[] { "Ð Ð¾Ð·ÐºÐ»Ð°Ð´", "ÐÐ°Ð»Ð°ÑÑÑÐ²Ð°Ð½Ð½Ñ" }
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };
    }

    public static InlineKeyboardMarkup Settings(UserSettings settings)
    {
        var notifications = settings.ReceiveNotifications ? "\U0001F7E2" : "\U0001F534";
        var buttons = new List<InlineKeyboardButton[]>
        {
            new[] { InlineButton("\U0000270F ÐÑÐºÐ½ÐµÐ¹Ð¼", "settings:nickname") }
        };

        if (settings.ReceiveNotifications)
        {
            buttons.Add(new[]
            {
                InlineButton($"{notifications} Ð¡Ð¿Ð¾Ð²ÑÑÐµÐ½Ð½Ñ", "settings:notifications"),
                InlineButton($"\U000023F0 ÐÐ° {(int)settings.TimeBeforeClassToNotify} ÑÐ². Ð´Ð¾ ÐÐ°Ñ", "settings:time_before_class") 
            });
        }
        else
        {
            buttons.Add(new[] { InlineButton($"{notifications} Ð¡Ð¿Ð¾Ð²ÑÑÐµÐ½Ð½Ñ", "settings:notifications") });
        }

        buttons.Add(new[] { InlineButton("\U0001F5D1 ÐÑÐ¾ÑÑÐ»Ñ", "settings:profile") });
        buttons.Add(new[] { InlineButton("\U0001F3E0 ÐÐ¾Ð»Ð¾Ð²Ð½Ðµ ÐÐµÐ½Ñ", "settings:main_menu") });

        return new InlineKeyboardMarkup(buttons);
    }

    public static InlineKeyboardMarkup YesLater()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineButton("Ð¢Ð°Ðº", "settings:yes"),
                InlineButton("ÐÑÐ·Ð½ÑÑÐµ", "settings:later")
            }
        });
    }

    public static InlineKeyboardMarkup RemoveCancel()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineButton("ÐÐ¸Ð´Ð°Ð»Ð¸ÑÐ¸", "settings:remove"),
                InlineButton("Ð¡ÐºÐ°ÑÑÐ²Ð°ÑÐ¸", "settings:cancel")
            }
        });
    }

    private static InlineKeyboardButton InlineButton(string text, string callbackData)
    {
        return InlineKeyboardButton.WithCallbackData(text, callbackData);
    }
}
