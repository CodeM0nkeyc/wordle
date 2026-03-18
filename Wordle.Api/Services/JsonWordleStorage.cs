namespace Wordle.Api.Services;

internal sealed class JsonWordleStorage : IWordleStorage, IDisposable
{
    private const string _wordKey = "word";
    private const string _oldWordKey = "oldWord";

    private bool _disposed = false;

    private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
    private readonly FileStream _wordFile;
    private readonly IMemoryCache _cache;

    public JsonWordleStorage(IMemoryCache cache)
    {
        _cache = cache;

        bool initJsonFile = false;

        if (!File.Exists("word.json"))
        {
            initJsonFile = true;
        }

        _wordFile = new FileStream("word.json", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read, 4096, true);

        if (initJsonFile)
        {
            InitWordJsonFile();
        }
    }

    ~JsonWordleStorage()
    {
        Dispose(false);
    }

    public async Task<WordleEntity?> GetWordAsync()
    {
        if (_cache.TryGetValue(_wordKey, out string? cachedWord) &&
            _cache.TryGetValue(_wordKey, out string? oldCachedWord))
        {
            return new WordleEntity()
            {
                CurrentWord = cachedWord!,
                OldWord = oldCachedWord!,
            };
        }

        await _lock.WaitAsync();

        try
        {
            WordleEntity? wordleEntity = await JsonSerializer.DeserializeAsync<WordleEntity>(_wordFile);

            return wordleEntity;
        }
        finally
        {
            _wordFile.Position = 0;
            _lock.Release();
        }
    }

    public async Task SetWordAsync(string word)
    {
        WordleEntity? oldWordleEntity = await GetWordAsync();
        WordleEntity newWordleEntity = new WordleEntity()
        {
            CurrentWord = word,
            OldWord = oldWordleEntity?.CurrentWord ?? word
        };

        await _lock.WaitAsync();

        try
        {
            await JsonSerializer.SerializeAsync(_wordFile, newWordleEntity);

            _cache.Set(_wordKey, newWordleEntity.CurrentWord);
            _cache.Set(_oldWordKey, newWordleEntity.OldWord);
        }
        finally
        {
            _wordFile.Position = 0;
            _lock.Release();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _wordFile.Dispose();
            _lock.Dispose();
        }

        _disposed = true;
    }

    private void InitWordJsonFile()
    {
        _wordFile.Write(Encoding.UTF8.GetBytes("{}"));
        _wordFile.Flush();
        _wordFile.Position = 0;
    }
}
