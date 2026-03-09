namespace Wordle.Api.Features.Persistence;

public class FileWordleStorage : IWordleStorage, IDisposable
{
    private bool _disposed = false;

    private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
    private readonly FileStream _wordFile;

    public FileWordleStorage()
    {
        _wordFile = new FileStream("word.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read, 4096, true);
    }

    public async Task<string> GetWordAsync()
    {
        await _lock.WaitAsync();

        StreamReader streamReader = new StreamReader(_wordFile, leaveOpen: true);

        try
        {
            string word = await streamReader.ReadToEndAsync();

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
