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

        char[] refWordArr = refWord.ToCharArray();

        int length = input.Length;

        (char Letter, LetterState State)[] states = new (char, LetterState)[length];
        Dictionary<char, int> presentCount = new Dictionary<char, int>();
        bool init = false;

        for (int i = 0; i < length; i++)
        {
            char letter = input[i];

            if (letter == refWordArr[i])
            {
                states[i] = (letter, LetterState.Guessed);
                refWordArr[i] = (char)0;

                continue;
            }

            for (int j = 0; j < length; j++)
            {
                if (!init)
                {
                    presentCount[input[j]] = 0;
                }

                if (letter == refWordArr[j])
                {
                    if (states[j].State == LetterState.Guessed)
                    {
                        presentCount[letter] -= 1;
                        continue;
                    }

                    presentCount[letter] += 1;

                    if (presentCount[letter] > 0)
                    {
                        states[i] = (letter, LetterState.Present);
                    }
                }
                else if (presentCount[letter] < 1)
                {
                    states[i] = (letter, LetterState.Wrong);
                }
            }

            init = true;
        }

        return states.Select(x => x.State).ToArray();

        //return states.Select(x =>
        //{
        //    if (x.State == LetterState.Present)
        //    {
        //        return sourceArr.Contains(x.Letter) ? LetterState.Present : LetterState.Wrong;
        //    }

        //    return x.State;
        //}).ToArray();
    }
}
