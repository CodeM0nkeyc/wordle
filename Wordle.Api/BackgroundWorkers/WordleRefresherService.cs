namespace Wordle.Api.BackgroundWorkers;

internal sealed class WordleRefresherService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IWordleStorage _wordleStorage;
    private readonly ILogger<WordleRefresherService> _logger;

    public WordleRefresherService(
        IServiceProvider serviceProvider,
        IWordleStorage wordleStorage,
        ILogger<WordleRefresherService> logger)
    {
        _serviceProvider = serviceProvider;
        _wordleStorage = wordleStorage;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting wordle refresher service.");

        await UpdateWordAsync();

        long delay = (WordleDefaults.StartRefreshUtc - DateTime.UtcNow).Ticks % (TimeSpan.TicksPerHour * 24);

        await Task.Delay(TimeSpan.FromTicks(delay));

        using PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromHours(24));

        while (true)
        {
            await UpdateWordAsync();
            await periodicTimer.WaitForNextTickAsync(stoppingToken);
        }
    }

    private async Task UpdateWordAsync()
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        var wordleApiClient = 
            scope.ServiceProvider.GetRequiredService<IWordleApiClient>();

        string word = await wordleApiClient.GetRandomWordAsync(WordleDefaults.Length);
        await _wordleStorage.SetWordAsync(word);

        Console.WriteLine(word);
    }
}
