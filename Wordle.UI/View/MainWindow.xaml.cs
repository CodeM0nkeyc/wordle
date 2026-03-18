namespace Wordle.UI.View;

public partial class MainWindow : Window, IDisposable
{
    private bool _disposed = false;

    private readonly IServiceScope _serviceScope;

    public MainWindow()
    {
        InitializeComponent();
        _serviceScope = App.Current.Services.CreateScope();
        ViewModel = _serviceScope.ServiceProvider.GetRequiredService<MainViewModel>();
        DataContext = ViewModel;
    }

    ~MainWindow()
    {
        Dispose(false);
    }

    public MainViewModel ViewModel { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        Dispose();
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _serviceScope.Dispose();
        }

        _disposed = true;
    }
}