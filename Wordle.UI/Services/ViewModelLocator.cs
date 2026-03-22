namespace Wordle.UI.Services;

public static class ViewModelLocator
{
    public static MainViewModel MainViewModel => App.Current.Services.GetRequiredService<MainViewModel>();
}
