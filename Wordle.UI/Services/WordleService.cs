namespace Wordle.UI.Services;

public class WordleService : IWordleService
{
    public LetterState[] CheckWord(string source, string word)
    {
        char[] sourceArr = source.ToUpper().ToCharArray();
        word = word.ToUpper();

        if (source.Length != word.Length)
        {
            throw new ArgumentException("Word length is not equal to source length", nameof(word));
        }

        int length = word.Length;

        (char Letter, LetterState State)[] states = new (char, LetterState)[length];

        for (int i = 0; i < length; i++)
        {
            if (word[i] == sourceArr[i])
            {
                states[i] = (word[i], LetterState.Guessed);
                sourceArr[i] = (char)0;
            }
            else if (sourceArr.Contains(word[i]))
            {
                states[i] = (word[i], LetterState.Present);
            }
            else
            {
                states[i] = (word[i], LetterState.Wrong);
            }
        }

        return states.Select(x =>
        {
            if (x.State == LetterState.Present)
            {
                return sourceArr.Contains(x.Letter) ? LetterState.Present : LetterState.Wrong;
            }

            return x.State;
        }).ToArray();
    }
}
