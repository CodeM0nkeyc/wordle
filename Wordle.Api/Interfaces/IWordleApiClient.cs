namespace Wordle.Api.Interfaces;

public interface IWordleApiClient
{
    public Task<string> GetRandomWordAsync(int wordLength);
}
