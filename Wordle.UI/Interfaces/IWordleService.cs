namespace Wordle.UI.Interfaces;

public interface IWordleService
{
    public LetterState[] CheckWord(string refWord, string input);
}
