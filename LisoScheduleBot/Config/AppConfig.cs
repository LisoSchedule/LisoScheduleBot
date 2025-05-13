using DotNetEnv;

namespace LisoScheduleBot.Config;

public class AppConfig
{
    public string BotToken { get; }
    public string UsersJson { get; }
    public string UserSettingsJson { get; }
    public string GroupsJson { get; }

    public AppConfig(string? path = null)
    {
        Env.Load(path);
        BotToken = Env.GetString(EnvKeys.BOT_TOKEN.ToString()) ?? 
            throw new InvalidDataException("BOT_TOKEN is null.");
        UsersJson = Path.Combine(AppContext.BaseDirectory, Env.GetString(EnvKeys.USERS_JSON.ToString())) ?? 
            throw new InvalidDataException("USERS_JSON is null.");
        UserSettingsJson = Path.Combine(AppContext.BaseDirectory, Env.GetString(EnvKeys.USER_SETTINGS_JSON.ToString())) ??
            throw new InvalidDataException("USER_SETTINGS_JSON is null.");
        GroupsJson = Path.Combine(AppContext.BaseDirectory, Env.GetString(EnvKeys.GROUPS_JSON.ToString())) ?? 
            throw new InvalidDataException("GROUPS_JSON is null.");
    }
}
