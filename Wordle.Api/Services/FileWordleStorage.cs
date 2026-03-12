namespace Wordle.Api.Services;

internal sealed class FileWordleStorage : IWordleStorage, IDisposable
{
    private const string _cacheKey = "word";
    private bool _disposed = false;

    private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
    private readonly FileStream _wordFile;
    private readonly IMemoryCache _cache;

    public FileWordleStorage(IMemoryCache cache)
    {
        _cache = cache;
        _wordFile = new FileStream("word.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read, 4096, true);
    }

    ~FileWordleStorage()
    {
        Dispose(false);
    }

    public async Task<string> GetWordAsync()
    {
        if (_cache.TryGetValue(_cacheKey, out string cachedWord))
        {
            return cachedWord!;
        }

        await _lock.WaitAsync();

        StreamReader streamReader = new StreamReader(_wordFile, leaveOpen: true);

        try
        {
            string word = await streamReader.ReadToEndAsync();
            _cache.Set(_cacheKey, word);

            return word;
        }
        finally
        {
            streamReader.Close();
            _wordFile.Position = 0;
            _lock.Release();
        }
    }

    public async Task SetWordAsync(string word)
    {
        await _lock.WaitAsync();

        StreamWriter streamWriter = new StreamWriter(_wordFile, leaveOpen: true);

        try
        {
            await streamWriter.WriteAsync(word);
            _cache.Set(_cacheKey, word);
        }
        finally
        {
            streamWriter.Close();
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
}
