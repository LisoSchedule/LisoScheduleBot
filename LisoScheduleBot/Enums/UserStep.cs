using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LisoScheduleBot.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum UserStep
{
    // Registration
    ChooseNickname,
    ChoosingNickname,
    ChooseGroup,
    ChooseSubGroup,

    // Main Menu
    MainMenu
}
