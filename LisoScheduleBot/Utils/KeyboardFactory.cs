using Telegram.Bot.Types.ReplyMarkups;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Models;

namespace LisoScheduleBot.Utils;

public static class KeyboardFactory
{
    public static ReplyKeyboardMarkup StartOver()
    {
        return new ReplyKeyboardMarkup(new[]
        {
            new KeyboardButton[] { $"{Emoji.Refresh} Почати заново" }
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
                InlineButton($"{Emoji.ThumbsUp} Так", "registration:yes"),
                InlineButton($"{Emoji.ThumbsDown} Пізніше", "registration:later")
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
            .Select(group => InlineButton(EnumConverter<GroupName>.EnumToString(group.Name!), $"registration:group_name:{group.Name}"))
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
            new KeyboardButton[] { $"{Emoji.OpenBook} Розклад", $"{Emoji.Gear} Налаштування" }
        })
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };
    }

    public static InlineKeyboardMarkup Settings(UserSettings settings)
    {
        var notifications = settings.ReceiveNotifications ? Emoji.GreenCircle : Emoji.RedCircle;
        var buttons = new List<InlineKeyboardButton[]>
        {
            new[] { InlineButton($"{Emoji.Pencil} Нікнейм", "settings:nickname") }
        };

        if (settings.ReceiveNotifications)
        {
            buttons.Add(new[]
            {
                InlineButton($"{notifications} Сповіщення", "settings:notifications"),
                InlineButton($"{Emoji.Clock} За {(int)settings.TimeBeforeClassToNotify} хв. до Пар", "settings:time_before_class") 
            });
        }
        else
        {
            buttons.Add(new[] { InlineButton($"{notifications} Сповіщення", "settings:notifications") });
        }

        buttons.Add(new[] { InlineButton($"{Emoji.TrashCan} Профіль", "settings:profile") });
        buttons.Add(new[] { InlineButton($"{Emoji.House} Головне Меню", "settings:main_menu") });

        return new InlineKeyboardMarkup(buttons);
    }

    public static InlineKeyboardMarkup YesLater()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineButton($"{Emoji.ThumbsUp} Так", "settings:yes"),
                InlineButton($"{Emoji.ThumbsDown} Пізніше", "settings:later")
            }
        });
    }

    public static InlineKeyboardMarkup RemoveCancel()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineButton($"{Emoji.PersonWithTrash} Видалити", "settings:remove"),
                InlineButton($"{Emoji.CrossMark} Скасувати", "settings:cancel")
            }
        });
    }

    private static InlineKeyboardButton InlineButton(string text, string callbackData)
    {
        return InlineKeyboardButton.WithCallbackData(text, callbackData);
    }
}
