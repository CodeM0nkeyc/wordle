namespace Wordle.UI.Interfaces;

public interface IWordleService
{
    public LetterState[] CheckWord(string source, string word);
}
