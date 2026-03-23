namespace Wordle.UI;

public partial class App : Application
{
    public new static App Current => (App)Application.Current;

    public IServiceProvider Services => ServiceLocator.Services;

    public App()
    {
        DispatcherUnhandledException += OnDispatcherUnhandledException;
    }

    private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        string exceptionMessage = $"{e.Exception.ToString()}";
        MessageBox.Show(exceptionMessage);
    }
}
