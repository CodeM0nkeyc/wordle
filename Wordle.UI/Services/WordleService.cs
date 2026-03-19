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
        Dictionary<char, int> presentCount = new Dictionary<char, int>();
        bool init = false;

        for (int i = 0; i < length; i++)
        {
            char letter = input[i];

            if (letter == refWord[i])
            {
                states[i] = LetterState.Guessed;
                continue;
            }

            for (int j = 0; j < length; j++)
            {
                if (!init)
                    presentCount[input[j]] = 0;

                if (letter == refWord[j])
                {
                    if (states[j] == LetterState.Guessed)
                    {
                        presentCount[letter] -= 1;
                        continue;
                    }

                    presentCount[letter] += 1;

                    if (presentCount[letter] > 0)
                        states[i] = LetterState.Present;
                }
                else if (presentCount[letter] < 1)
                {
                    states[i] = LetterState.Wrong;
                }
            }

            init = true;
        }

        return states;
    }
}
