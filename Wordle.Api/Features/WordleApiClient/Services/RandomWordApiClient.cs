namespace Wordle.Api.Features.WordleApiClient.Services;

public class RandomWordApiClient : WordleApiClient
{
    public RandomWordApiClient(IHttpClientFactory httpClientFactory) 
        : base(@"https://random-word-api.herokuapp.com", httpClientFactory)
    {
    }

    public override async Task<string> GetRandomWordAsync()
    {
        HttpResponseMessage response = await HttpClient.GetAsync($"word?length={wordLength}");
        response.EnsureSuccessStatusCode();

        string[]? responseObject = await response.Content.ReadFromJsonAsync<string[]>();

        if (responseObject is null || responseObject.Length == 0)
        {
            throw new InvalidOperationException(
                $"Api call to {response.RequestMessage?.RequestUri?.ToString()} returned nothing.");
        }

        return responseObject[0];
    }
}
