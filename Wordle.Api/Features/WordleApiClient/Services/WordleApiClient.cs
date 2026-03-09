namespace Wordle.Api.Features.WordleApiClient.Services;

public abstract class WordleApiClient
{
    protected const int wordLength = 5;

    protected string ApiUrl { get; }

    protected HttpClient HttpClient { get; }

    protected WordleApiClient(string apiUrl, IHttpClientFactory httpClientFactory)
    {
        ApiUrl = apiUrl;

        HttpClient = httpClientFactory.CreateClient("WordleClient");
        HttpClient.BaseAddress = new Uri(ApiUrl);
    }

    public abstract Task<string> GetRandomWordAsync();
}
