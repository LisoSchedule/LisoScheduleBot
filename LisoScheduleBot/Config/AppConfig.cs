using DotNetEnv;

namespace LisoScheduleBot.Config;

public class AppConfig
{
    public string BotToken { get; }
    public string AppPassword { get; }
    public string HangfirePassword { get; }
    public string HangfireTimezone { get; }
    public string ClassroomsJson { get; }
    public string CodesJson { get; }
    public string GroupsJson { get; }
    public string LessonRecurrencesJson { get; }
    public string LessonsJson { get; }
    public string SubjectsJson { get; }
    public string TeachersJson { get; }
    public string UserSettingsJson { get; }
    public string UsersJson { get; }

    public AppConfig(string? path = null)
    {
        Env.Load(path);

        BotToken = Env.GetString(EnvKeys.BOT_TOKEN.ToString()) ?? 
            throw new InvalidDataException("BOT_TOKEN is null.");

        AppPassword = Env.GetString(EnvKeys.APP_PASSWORD.ToString()) ??
            throw new InvalidDataException("APP_PASSWORD is null.");

        HangfirePassword = Env.GetString("HANGFIRE_PASSWORD") ??
            throw new InvalidDataException("HANGFIRE_PASSWORD is null.");

        HangfireTimezone = Env.GetString("HANGFIRE_TIMEZONE");

        ClassroomsJson = Path.Combine(AppContext.BaseDirectory, Env.GetString(EnvKeys.CLASSROOMS_JSON.ToString())) ??
            throw new InvalidDataException("CLASSROOMS_JSON is null.");

        CodesJson = Path.Combine(AppContext.BaseDirectory, Env.GetString(EnvKeys.CODES_JSON.ToString())) ??
            throw new InvalidDataException("CODES_JSON is null.");

        GroupsJson = Path.Combine(AppContext.BaseDirectory, Env.GetString(EnvKeys.GROUPS_JSON.ToString())) ?? 
            throw new InvalidDataException("GROUPS_JSON is null.");

        LessonRecurrencesJson = Path.Combine(AppContext.BaseDirectory, Env.GetString(EnvKeys.LESSON_RECURRENCES_JSON.ToString())) ??
            throw new InvalidDataException("LESSON_RECURRENCES_JSON is null.");

        LessonsJson = Path.Combine(AppContext.BaseDirectory, Env.GetString(EnvKeys.LESSONS_JSON.ToString())) ?? 
            throw new InvalidDataException("LESSONS_JSON is null.");

        SubjectsJson = Path.Combine(AppContext.BaseDirectory, Env.GetString(EnvKeys.SUBJECTS_JSON.ToString())) ??
            throw new InvalidDataException("SUBJECTS_JSON is null.");

        TeachersJson = Path.Combine(AppContext.BaseDirectory, Env.GetString(EnvKeys.TEACHERS_JSON.ToString())) ??
            throw new InvalidDataException("TEACHERS_JSON is null.");

        UserSettingsJson = Path.Combine(AppContext.BaseDirectory, Env.GetString(EnvKeys.USER_SETTINGS_JSON.ToString())) ??
            throw new InvalidDataException("USER_SETTINGS_JSON is null.");

        UsersJson = Path.Combine(AppContext.BaseDirectory, Env.GetString(EnvKeys.USERS_JSON.ToString())) ?? 
            throw new InvalidDataException("USERS_JSON is null.");
    }
}
