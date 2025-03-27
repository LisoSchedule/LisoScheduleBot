using LisoScheduleBot.Config;
using LisoScheduleBot.Handlers;
using LisoScheduleBot.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

var appConfig = new AppConfig();
builder.Services.AddSingleton(appConfig);

builder.Services.AddSingleton<BotUpdateHandler>();
builder.Services.AddHostedService<BotService>();

var app = builder.Build();

app.MapGet("/", () => "Bot is running...");
app.Run();
