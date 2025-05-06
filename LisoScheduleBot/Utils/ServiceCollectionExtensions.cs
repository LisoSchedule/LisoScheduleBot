namespace LisoScheduleBot.Utils;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSingletonWithInterfaces<TService, TInterface1, TInterface2>(this IServiceCollection services)
        where TService : class, TInterface1, TInterface2
        where TInterface1 : class
        where TInterface2 : class
    {
        services.AddSingleton<TService>();
        services.AddSingleton<TInterface1>(provider => provider.GetRequiredService<TService>());
        services.AddSingleton<TInterface2>(provider => provider.GetRequiredService<TService>());

        return services;
    }
}
