namespace Wordle.UI.Services;

internal class WordleLocalApiClient : IWordleApiClient, IDisposable
{
    private bool _disposed = false;

    private const string _cacheKey = "word";

    protected readonly string apiUrl;
    protected readonly string apiKey;
    protected readonly int wordLength;

    protected readonly Lazy<HttpClient> httpClient;

    public WordleLocalApiClient(string apiUrl, string apiKey, int wordLength)
    {
        this.apiUrl = apiUrl;
        this.apiKey = apiKey;
        this.wordLength = wordLength;

        httpClient = new Lazy<HttpClient>(() =>
        {
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = new Uri(apiUrl),
                Timeout = TimeSpan.FromSeconds(10)
            };

            httpClient.DefaultRequestHeaders.Add("Authorization", $"ApiKey {apiKey}");

            return httpClient;
        }, false);
    }

    ~WordleLocalApiClient()
    {
        Dispose(false);
    }

    public virtual async Task<string> GetRandomWordAsync()
    {
        string? word = MemoryCache.Default.Get(_cacheKey) as string;

        if (word is not null)
        {
            return word;
        }

        var httpClient = this.httpClient.Value;
        var response = await httpClient.GetAsync("");

        response.EnsureSuccessStatusCode();

        word = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrEmpty(word) || word.Length != wordLength)
        {
            throw new InvalidOperationException($"Invalid string was returned from api: {word}");
        }

        DateTimeOffset dateTimeOffset = new DateTimeOffset().AddMinutes(10);
        MemoryCache.Default.Set(_cacheKey, word, dateTimeOffset);

        return word;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (httpClient.IsValueCreated)
        {
            httpClient.Value.Dispose();
        }

        _disposed = true;
    }
}
