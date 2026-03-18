namespace Wordle.Api.Abstractions;

internal interface IWordleStorage
{
    public Task SetWordAsync(string word);
    public Task<string> GetWordAsync();
}
