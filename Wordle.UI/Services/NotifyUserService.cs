namespace Wordle.UI.Services;

public class NotifyUserService : INotifyUserService
{
    public void NotifyByMessageBox(string message, string caption, bool warn)
    {
        MessageBoxButton button = MessageBoxButton.OK;
        MessageBoxImage image = warn ? MessageBoxImage.Warning : MessageBoxImage.Information;

        MessageBox.Show(message, caption, button, image);
    }
}
