namespace Wordle.Api.Misc.Settings;

// NOT USED
public interface IAppSettingsService : IAsyncDisposable
{
    public void UpdateAppSetting(string section, string key, string value);
    public void UpdateAppSetting(ReadOnlySpan<string> sections, string key, string value);
    public Task SaveChangesAsync();
}
