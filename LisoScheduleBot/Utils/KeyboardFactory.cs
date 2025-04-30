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

    public static InlineKeyboardMarkup YesLaterNicknames(string firstName, string? username = null)
    {
        var buttons = new List<InlineKeyboardButton[]>
        {
            new[]
            {
                InlineButton("Так", "registration:yes"),
                InlineButton("Пізніше", "registration:later")
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
            new KeyboardButton[] { "Розклад", "Налаштування" }
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
            new[] { InlineButton("\U0000270F Нікнейм", "settings:nickname") }
        };

        if (settings.ReceiveNotifications)
        {
            buttons.Add(new[]
            {
                InlineButton($"{notifications} Сповіщення", "settings:notifications"),
                InlineButton($"\U000023F0 За {(int)settings.TimeBeforeClassToNotify} хв. до Пар", "settings:time_before_class") 
            });
        }
        else
        {
            buttons.Add(new[] { InlineButton($"{notifications} Сповіщення", "settings:notifications") });
        }

        buttons.Add(new[] { InlineButton("\U0001F5D1 Профіль", "settings:profile") });
        buttons.Add(new[] { InlineButton("\U0001F3E0 Головне Меню", "settings:main_menu") });

        return new InlineKeyboardMarkup(buttons);
    }

    public static InlineKeyboardMarkup YesLater()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineButton("Так", "settings:yes"),
                InlineButton("Пізніше", "settings:later")
            }
        });
    }

    public static InlineKeyboardMarkup RemoveCancel()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineButton("Видалити", "settings:remove"),
                InlineButton("Скасувати", "settings:cancel")
            }
        });
    }

    private static InlineKeyboardButton InlineButton(string text, string callbackData)
    {
        return InlineKeyboardButton.WithCallbackData(text, callbackData);
    }
}
