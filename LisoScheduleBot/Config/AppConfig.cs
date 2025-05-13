using DotNetEnv;

namespace LisoScheduleBot.Config;

public class AppConfig
{
    public string BotToken { get; }

    public AppConfig(string? path = null)
    {
        Env.Load(path);
        BotToken = Env.GetString(EnvKeys.BOT_TOKEN.ToString()) ?? throw new InvalidDataException("BOT_TOKEN is null.");
    }
}
