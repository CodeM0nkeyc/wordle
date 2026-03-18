namespace Wordle.Api.Abstractions;

internal interface IWordleApiClient
{
    public abstract Task<string> GetRandomWordAsync();
}
