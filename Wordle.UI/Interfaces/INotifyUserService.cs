namespace Wordle.UI.Interfaces;

public interface INotifyUserService
{
    public void NotifyByMessageBox(string message, string caption, bool warn);
}
