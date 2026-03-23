namespace Wordle.UI.Interfaces;

public interface IWordleApiClient
{
    public Task<string> GetRandomWordAsync();
}
