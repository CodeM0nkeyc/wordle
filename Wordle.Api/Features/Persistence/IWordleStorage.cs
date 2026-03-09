namespace Wordle.Api.Features.Persistence;

public interface IWordleStorage
{
    public Task SetWordAsync(string word);
    public Task<string> GetWordAsync();
}
