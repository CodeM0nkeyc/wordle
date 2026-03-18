namespace Wordle.Api.Interfaces;

public interface IWordleStorage
{
    public Task SetWordAsync(string word);
    public Task<WordleEntity?> GetWordAsync();
}
