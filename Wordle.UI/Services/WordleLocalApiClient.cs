namespace Wordle.UI.Services;

internal class WordleLocalApiClient : IWordleApiClient, IDisposable
{
    private const string CacheKey = "word";

    private bool _disposed = false;

    private readonly string apiUrl;
    private readonly string apiKey;
    private readonly int wordLength;

    private readonly Lazy<HttpClient> httpClient;

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
            httpClient.DefaultRequestHeaders.Add("X-TimeZone", TimeZoneInfo.Local.BaseUtcOffset.ToString());

            return httpClient;
        }, false);
    }

    ~WordleLocalApiClient()
    {
        Dispose(false);
    }

    public virtual async Task<string> GetRandomWordAsync()
    {
        string? word = MemoryCache.Default.Get(CacheKey) as string;

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
        MemoryCache.Default.Set(CacheKey, word, dateTimeOffset);

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
