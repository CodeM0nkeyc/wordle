namespace Wordle.Api.Services;

internal sealed class RandomWordApiClient : IWordleApiClient
{
    private const string _apiUrl = @"https://random-words-api.kushcreates.com";

    private readonly HttpClient _httpClient;

    public RandomWordApiClient(IHttpClientFactory httpClientFactory) 
    {
        _httpClient = httpClientFactory.CreateClient("WordleClient");
        _httpClient.BaseAddress = new Uri(_apiUrl);
    }

    public async Task<string> GetRandomWordAsync(int wordLength)
    {
        HttpResponseMessage response = 
            await _httpClient.GetAsync($"api?language=en&category=wordle&length={wordLength}&words=1");
        response.EnsureSuccessStatusCode();

        WordInfo[]? responseObject = await response.Content.ReadFromJsonAsync<WordInfo[]>();

        if (responseObject is null || responseObject.Length == 0)
        {
            throw new InvalidOperationException(
                $"Api call to {response.RequestMessage?.RequestUri?.ToString()} returned nothing.");
        }

        return responseObject[0].Value;
    }
}
