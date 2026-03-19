namespace Wordle.UI.Services;

public class WordleService : IWordleService
{
    public LetterState[] CheckWord(string refWord, string input)
    {
        if (refWord.Length != input.Length)
        {
            throw new ArgumentException("Word length is not equal to source length", nameof(input));
        }

        refWord = refWord.ToUpper();
        input = input.ToUpper();

        int length = input.Length;

        LetterState[] states = new LetterState[length];

        for (int i = 0; i < length; i++)
        {
            char letter = input[i];
            char refLetter = refWord[i];

            if (letter == refLetter)
            {
                states[i] = LetterState.Guessed;
                continue;
            }

            for (int j = 0; j < length; j++)
            {
                if (states[j] == LetterState.Guessed || states[j] == LetterState.Present)
                {
                    continue;
                }

                if (refLetter == input[j])
                {
                    states[j] = LetterState.Present;
                    break;
                }
                
                states[j] = LetterState.Wrong;
            }
        }

        return states;
    }
}
