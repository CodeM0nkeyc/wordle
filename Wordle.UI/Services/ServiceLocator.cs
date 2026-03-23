namespace Wordle.UI.Services;

public static class ServiceLocator
{
    static ServiceLocator()
    {
        Services = CreateServiceProvider();
    }

    public static IServiceProvider Services { get; }

    public static MainViewModel MainViewModel => App.Current.Services.GetRequiredService<MainViewModel>();

    private static IServiceProvider CreateServiceProvider()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<INotifyUserService, NotifyUserService>();
        services.AddSingleton<IWordleService, WordleService>();

        services.AddSingleton<IWordleApiClient, WordleLocalApiClient>(p =>
        {
            var wordleApiClient = new WordleLocalApiClient(
                Defaults.BaseApiUrl + "/wordle", Defaults.ApiKey, Defaults.WordLength);

            return wordleApiClient;
        });

        services.AddSingleton<MainViewModel>();

        return services.BuildServiceProvider(true);
    }
}
