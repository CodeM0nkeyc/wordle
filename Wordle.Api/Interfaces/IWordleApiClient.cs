namespace Wordle.Api.Interfaces;

internal interface IWordleApiClient
{
    public abstract Task<string> GetRandomWordAsync(int wordLength);
}
