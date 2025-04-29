using LisoScheduleBot.Config;
using LisoScheduleBot.Dispatchers;
using LisoScheduleBot.Handlers;
using LisoScheduleBot.Handlers.Callback;
using LisoScheduleBot.Handlers.Registration;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Repositories;
using LisoScheduleBot.Services;
using Telegram.Bot;

namespace LisoScheduleBot
{
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
            services.AddSingleton<BotUpdateHandler>();
            services.AddSingleton<MessageHandler>();

            // Repositories
            services.AddSingleton<JsonGroupRepository>();
            services.AddSingleton<JsonUserRepository>();
            services.AddSingleton<JsonUserSettingsRepository>();

            // Services
            services.AddHostedService<BotService>();
            services.AddSingleton<IGroupService, GroupService>();
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IUserSettingsService, UserSettingsService>();
        }

        public void Configure(WebApplication app)
        {
            app.MapGet("/", () => "Bot is running...");
        }
    }
}
