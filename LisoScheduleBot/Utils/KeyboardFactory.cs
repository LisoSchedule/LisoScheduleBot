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
            new[] { InlineButton($"{Emoji.Pen} Нікнейм", "settings:nickname") }
        };

        if (settings.ReceiveNotifications)
        {
            buttons.Add(new[]
            {
                InlineButton($"{notifications} Сповіщення", "settings:notifications"),
                InlineButton($"{Emoji.Clock} {(int)settings.TimeBeforeClassToNotify} хв. до Пари", "settings:time_before_class") 
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
                InlineButton($"{Emoji.TrashCan} Видалити", "settings:remove"),
                InlineButton($"{Emoji.CrossMark} Скасувати", "settings:cancel")
            }
        });
    }

    public static InlineKeyboardMarkup TodayFromDate()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineButton($"{Emoji.Date} Сьогодні", "schedule:today"),
                InlineButton($"{Emoji.Calendar} За Днем", "schedule:from_date")
            },
            new[]
            {
                InlineButton($"{Emoji.House} Головне Меню", "schedule:main_menu")
            }
        });
    }

    public static InlineKeyboardMarkup TodayBack()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[] { InlineButton($"{Emoji.OpenBook} Розклад", "schedule:back") }
        });
    }

    public static InlineKeyboardMarkup DateBack(DateOnly date)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var daysToMondayDate = ((int)date.DayOfWeek + 6) % 7;
        var daysToMondayToday = ((int)today.DayOfWeek + 6) % 7;

        var mondayOfDate = date.AddDays(-daysToMondayDate);
        var mondayOfToday = today.AddDays(-daysToMondayToday);

        var callback = mondayOfDate == mondayOfToday ? "this_week" : "next_week";

        return new InlineKeyboardMarkup(new[]
        {
            new[] 
            { 
                InlineButton($"{Emoji.ArrowLeft} Назад", $"schedule:{callback}"),
                InlineButton($"{Emoji.OpenBook} Розклад", "schedule:back")
            }
        });
    }

    public static InlineKeyboardMarkup WeekButtons()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var daysToMonday = ((int)today.DayOfWeek + 6) % 7;
        var monday = today.AddDays(-daysToMonday);

        var days = new[]
        {
            ("Понеділок", 0),
            ("Вівторок", 1),
            ("Середа", 2),
            ("Четвер", 3),
            ("П'ятниця", 4)
        };

        var buttons = new List<InlineKeyboardButton[]>
        {
            new[]
            {
                InlineButton(days[0].Item1, $"schedule:date:{monday.AddDays(days[0].Item2)}"),
                InlineButton(days[1].Item1, $"schedule:date:{monday.AddDays(days[1].Item2)}")
            },
            new[]
            {
                InlineButton(days[2].Item1, $"schedule:date:{monday.AddDays(days[2].Item2)}"),
                InlineButton(days[3].Item1, $"schedule:date:{monday.AddDays(days[3].Item2)}")
            },
            new[]
            {
                InlineButton(days[4].Item1, $"schedule:date:{monday.AddDays(days[4].Item2)}")
            },
            new[]
            {
                InlineButton($"{Emoji.OpenBook} Розклад", "schedule:back"),
                InlineButton($"Далі {Emoji.ArrowRight}", "schedule:next")
            }
        };

        return new InlineKeyboardMarkup(buttons);
    }

    public static InlineKeyboardMarkup NextWeekButtons()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var daysToMonday = ((int)today.DayOfWeek + 6) % 7;
        var monday = today.AddDays(-daysToMonday).AddDays(7);

        var days = new[]
        {
            ($"{monday:dd.MM.yy}", 0),
            ($"{monday.AddDays(1):dd.MM.yy}", 1),
            ($"{monday.AddDays(2):dd.MM.yy}", 2),
            ($"{monday.AddDays(3):dd.MM.yy}", 3),
            ($"{monday.AddDays(4):dd.MM.yy}", 4)
        };

        var buttons = new List<InlineKeyboardButton[]>
        {
            new[]
            {
                InlineButton(days[0].Item1, $"schedule:date:{monday.AddDays(days[0].Item2)}"),
                InlineButton(days[1].Item1, $"schedule:date:{monday.AddDays(days[1].Item2)}")
            },
            new[]
            {
                InlineButton(days[2].Item1, $"schedule:date:{monday.AddDays(days[2].Item2)}"),
                InlineButton(days[3].Item1, $"schedule:date:{monday.AddDays(days[3].Item2)}")
            },
            new[]
            {
                InlineButton(days[4].Item1, $"schedule:date:{monday.AddDays(days[4].Item2)}")
            },
            new[]
            {
                InlineButton($"{Emoji.ArrowLeft} Назад", "schedule:previous"),
                InlineButton($"{Emoji.OpenBook} Розклад", "schedule:back"),
            }
        };

        return new InlineKeyboardMarkup(buttons);
    }

    private static InlineKeyboardButton InlineButton(string text, string callbackData)
    {
        return InlineKeyboardButton.WithCallbackData(text, callbackData);
    }
}
