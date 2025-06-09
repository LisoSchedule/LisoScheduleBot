using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LisoScheduleBot.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum UserStep
{
    // Registration
    StartOver,
    ChooseNickname,
    ChoosingNickname,
    ChooseGroup,
    ChooseSubGroup,

    // Main Menu
    MainMenu,

    // Schedule
    ChooseSchedule,
    ScheduleToday,
    ScheduleThisWeek,
    ScheduleNextWeek,
    ScheduleDate,

    //Settings
    ChangeSettings,
    ChangeNickname,
    ChangingNickname,
    ChangeEmail,
    ChangingEmail,
    VerifyEmail,
    VerifyingEmail,
    ChangeGroup,
    ChangingGroup,
    ChangingSubGroup,
    RemoveProfile
}
