namespace Wordle.UI.Tests;

public class WordleServiceTest
{
    private readonly IWordleService _wordleService = new WordleService();

    [Theory]
    [InlineData("fffff", "fffff")]
    [InlineData("htqhe", "htqhe")]
    [InlineData("apple", "apple")]
    public void WordleService_WordGuessedTest(string refWord, string input)
    {
        LetterState[] expectedStates = Enumerable.Repeat(LetterState.Guessed, refWord.Length).ToArray();
        LetterState[] states = _wordleService.CheckWord(refWord, input);

        Assert.True(expectedStates.Length == states.Length);
        Assert.Equal(expectedStates, states);
    }

    [Theory]
    [InlineData("fffff", "eeeee")]
    [InlineData("htnqh", "escre")]
    [InlineData("apple", "onion")]
    public void WordleService_WordNotGuessedTest(string refWord, string input)
    {
        LetterState[] expectedStates = Enumerable.Repeat(LetterState.Wrong, refWord.Length).ToArray();
        LetterState[] states = _wordleService.CheckWord(refWord, input);

        Assert.True(expectedStates.Length == states.Length);
        Assert.Equal(expectedStates, states);
    }

    [Theory]
    [InlineData("ssfss", "fffff")]
    [InlineData("ecfyb", "sfffr")]
    [InlineData("efffb", "yrfwr")]
    public void WordleService_WordPartialyGuessedTest_1(string refWord, string input)
    {
        LetterState[] expectedStates = Enumerable.Repeat(LetterState.Wrong, refWord.Length).ToArray();
        expectedStates[2] = LetterState.Guessed;
        LetterState[] states = _wordleService.CheckWord(refWord, input);

        Assert.True(expectedStates.Length == states.Length);
        Assert.Equal(expectedStates, states);
    }

    [Theory]
    [InlineData("sfsfs", "ffffs")]
    [InlineData("ncfyb", "yceyb")]
    [InlineData("ffefb", "yfrfb")]
    public void WordleService_WordPartialyGuessedTest_2(string refWord, string input)
    {
        LetterState[] expectedStates = new LetterState[]
        {
            LetterState.Wrong,
            LetterState.Guessed,
            LetterState.Wrong,
            LetterState.Guessed,
            LetterState.Guessed
        };

        LetterState[] states = _wordleService.CheckWord(refWord, input);

        Assert.True(expectedStates.Length == states.Length);
        Assert.Equal(expectedStates, states);
    }

    [Theory]
    [InlineData("apple", "alley")]
    [InlineData("frace", "fased")]
    [InlineData("drite", "dived")]
    public void WordleService_WordPartialyGuessedTestWithPresent_1(string refWord, string input)
    {
        LetterState[] expectedStates = new LetterState[]
        {
            LetterState.Guessed,
            LetterState.Present,
            LetterState.Wrong,
            LetterState.Present,
            LetterState.Wrong
        };

        LetterState[] states = _wordleService.CheckWord(refWord, input);

        Assert.True(expectedStates.Length == states.Length);
        Assert.Equal(expectedStates, states);
    }

    [Theory]
    [InlineData("aylee", "alyey")]
    [InlineData("frace", "farcd")]
    [InlineData("drite", "dertd")]
    public void WordleService_WordPartialyGuessedTestWithPresent_2(string refWord, string input)
    {
        LetterState[] expectedStates = new LetterState[]
        {
            LetterState.Guessed,
            LetterState.Present,
            LetterState.Present,
            LetterState.Guessed,
            LetterState.Wrong
        };

        LetterState[] states = _wordleService.CheckWord(refWord, input);

        Assert.True(expectedStates.Length == states.Length);
        Assert.Equal(expectedStates, states);
    }
}
