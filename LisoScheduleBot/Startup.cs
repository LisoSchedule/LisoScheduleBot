using Telegram.Bot;
using LisoScheduleBot.Config;
using LisoScheduleBot.Dispatchers;
using LisoScheduleBot.Handlers;
using LisoScheduleBot.Handlers.Callback;
using LisoScheduleBot.Handlers.Registration;
using LisoScheduleBot.Handlers.Settings;
using LisoScheduleBot.Handlers.Schedule;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Models;
using LisoScheduleBot.Repositories;
using LisoScheduleBot.Services;
using LisoScheduleBot.Utils;

namespace LisoScheduleBot;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Logging
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });

        // Config
        var appConfig = new AppConfig();
        services.AddSingleton(appConfig);

        // Telegram Bot Client
        services.AddSingleton<ITelegramBotClient>(sp => new TelegramBotClient(appConfig.BotToken));

        // Dispatchers
        services.AddSingleton<CallbackDispatcher>();
        services.AddSingleton<UserStepDispatcher>();

        // Callback Handlers
        services.AddSingleton<ICallbackHandler, RegistrationCallbackHandler>();
        services.AddSingleton<ICallbackHandler, ScheduleCallbackHandler>();
        services.AddSingleton<ICallbackHandler, SettingsCallbackHandler>();

        // Handlers
        services.AddSingleton<IUserStepHandler, ChooseGroupHandler>();
        services.AddSingleton<IUserStepHandler, ChooseNicknameHandler>();
        services.AddSingleton<IUserStepHandler, ChooseSubGroupHandler>();
        services.AddSingleton<IUserStepHandler, ChoosingNicknameHandler>();
        services.AddSingleton<IUserStepHandler, StartOverHandler>();
        services.AddSingleton<IUserStepHandler, ChangeNicknameHandler>();
        services.AddSingleton<IUserStepHandler, ChangeSettingsHandler>();
        services.AddSingleton<IUserStepHandler, ChangingNicknameHandler>();
        services.AddSingleton<IUserStepHandler, RemoveProfileHandler>();
        services.AddSingleton<IUserStepHandler, ChooseScheduleHandler>();
        services.AddSingleton<IUserStepHandler, ScheduleDateHandler>();
        services.AddSingleton<IUserStepHandler, ScheduleNextWeekHandler>();
        services.AddSingleton<IUserStepHandler, ScheduleThisWeekHandler>();
        services.AddSingleton<IUserStepHandler, ScheduleTodayHandler>();
        services.AddSingleton<IUserStepHandler, MainMenuHandler>();
        services.AddSingleton<BotUpdateHandler>();
        services.AddSingleton<MessageHandler>();

        // Repositories
        services.AddSingleton<JsonClassroomRepository>();
        services.AddSingleton<JsonGroupRepository>();
        services.AddSingleton<JsonLessonRecurrenceRepository>();
        services.AddSingleton<JsonLessonRepository>();
        services.AddSingleton<JsonSubjectRepository>();
        services.AddSingleton<JsonTeacherRepository>();
        services.AddSingleton<JsonUserRepository>();
        services.AddSingleton<JsonUserSettingsRepository>();

        // Services
        services.AddHostedService<BotService>();
        services.AddSingleton<IService<Classroom>, JsonClassroomService>();
        services.AddSingletonWithInterfaces<JsonGroupService, IService<Group>, IGroupService>();
        services.AddSingleton<IService<LessonRecurrence>, JsonLessonRecurrenceService>();
        services.AddSingleton<IService<Lesson>, JsonLessonService>();
        services.AddSingleton<IScheduleService, JsonScheduleService>();
        services.AddSingleton<IService<Subject>, JsonSubjectService>();
        services.AddSingleton<IService<Teacher>, JsonTeacherService>();
        services.AddSingleton<IUserService, JsonUserService>();
        services.AddSingleton<IService<UserSettings>, JsonUserSettingsService>();
        services.AddSingleton<IMessageService, MessageService>();
    }

    public void Configure(WebApplication app)
    {
        app.MapGet("/", () => "Bot is running...");
    }
}
