namespace Wordle.UI;

public partial class App : Application
{
    public new static App Current => (App)Application.Current;

    public IServiceProvider Services { get; }

    public App()
    {
        Services = CreateServiceProvider();

        DispatcherUnhandledException += OnDispatcherUnhandledException;
    }

    private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        string exceptionMessage = $"{e.Exception.ToString()}";
        MessageBox.Show(exceptionMessage);
    }

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
