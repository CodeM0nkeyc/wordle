namespace Wordle.Api.Misc.Settings;

// NOT USED
public class AppSettingsService : IAppSettingsService
{
    private bool _disposed = false;
    private bool _changesSaved = true;

    private readonly string _appSettingsPath;

    private static readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

    private readonly Lazy<JsonNode> _rootNode;

    public AppSettingsService(string appSettingsPath)
    {
        _appSettingsPath = appSettingsPath;

        _rootNode = new Lazy<JsonNode>(() =>
        {
            _lock.Wait();

            try
            {
                string json = File.ReadAllText(_appSettingsPath);
                return JsonNode.Parse(json)!;
            }
            finally
            {
                _lock.Release();
            }
        }, false);
    }

    public void UpdateAppSetting(string section, string key, string value)
    {
        JsonNode root = _rootNode.Value;
        JsonNode? sectionNode = _rootNode.Value[section];

        if (sectionNode is not null)
        {
            sectionNode[key] = value;
        }
        else
        {
            root[section] = new JsonObject { [key] = value };
        }

        _changesSaved = false;
    }

    public void UpdateAppSetting(ReadOnlySpan<string> sections, string key, string value)
    {
        JsonNode root = _rootNode.Value;

        UpdateAppSettingRecursive(root, sections, key, value);

        _changesSaved = false;
    }

    private void UpdateAppSettingRecursive(JsonNode node, ReadOnlySpan<string> sections, string key, string value)
    {
        string section = sections[0];

        if (node[section] is null)
        {
            node[section] = new JsonObject();
        }

        JsonNode sectionNode = node[section]!;

        if (sections.Length > 1)
        {
            UpdateAppSettingRecursive(sectionNode, sections.Slice(1), key, value);
        }
        else
        {
            sectionNode[key] = value;
        }
    }

    public async Task SaveChangesAsync()
    {
        await WriteChangesAsync();

        _changesSaved = true;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        if (!_changesSaved)
        {
            await SaveChangesAsync();
        }

        _disposed = true;
    }

    private async Task WriteChangesAsync()
    {
        await _lock.WaitAsync();

        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            await File.WriteAllTextAsync(_appSettingsPath, _rootNode.Value.ToJsonString(options));
        }
        finally
        {
            _lock.Release();
        }
    }
}
