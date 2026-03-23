namespace Wordle.UI.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private string? _fetchedWord;

    private readonly INotifyUserService _notifyUserService;

    private readonly IWordleService _wordleService;
    private readonly IWordleApiClient _wordleApiClient;

    [ObservableProperty]
    private LinkedList<WordViewModel> _words;

    private LinkedListNode<WordViewModel> _currentWord;

    public MainViewModel(
        INotifyUserService notifyUserService,
        IWordleService wordleService,
        IWordleApiClient wordleApiClient)
    {
        _notifyUserService = notifyUserService;
        _wordleService = wordleService;
        _wordleApiClient = wordleApiClient;

        _words = new LinkedList<WordViewModel>(
            EnumerableHelper.RepeatUnique(() => new WordViewModel(Defaults.WordLength, this), 6));

        _currentWord = _words.First!;
        _currentWord.Value.Current = true;
    }

    public void NotifyWordChanged()
    {
        CheckWordCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanExecuteCheckWord))]
    private async Task CheckWordAsync()
    {
        if (_fetchedWord is null)
        {
            try
            {
                _fetchedWord = await _wordleApiClient.GetRandomWordAsync();
            }
            catch (Exception ex)
            {
                _notifyUserService.NotifyByMessageBox("Something went wrong. Please, try later.", "Error", true);
                return;
            }
        }

        WordViewModel currentWord = _currentWord.Value;

        LetterState[] letterStates = _wordleService.CheckWord(_fetchedWord!, currentWord.String);

        for (int i = 0; i < letterStates.Length; i++)
        {
            currentWord.Letters[i].State = letterStates[i];
        }
        if (currentWord.Guessed)
        {
            _currentWord.Value.Current = false;
            _notifyUserService.NotifyByMessageBox("Congratulations! You guessed the word.", "Congratulations", false);
            return;
        }

        bool isLastWord = !MoveToNextWord();

        if (isLastWord)
        {
            _notifyUserService.NotifyByMessageBox(
                $"You didn't guessed the word. It's {_fetchedWord}. Don't frustrate, you will get it another time.",
                "Will guess another time",
                false);
        }
    }

    private bool CanExecuteCheckWord()
    {
        WordViewModel currentWord = _currentWord.Value;
        return currentWord.LettersSpecified == currentWord.Length;
    }

    private bool MoveToNextWord()
    {
        _currentWord.Value.Current = false;

        if (_currentWord.Next is null)
        {
            return false;
        }

        _currentWord = _currentWord.Next;
        _currentWord.Value.Current = true;
        return true;
    }
}
