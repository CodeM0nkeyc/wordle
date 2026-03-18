namespace Wordle.UI.ViewModel;

public partial class WordViewModel : ObservableObject
{
    private readonly MainViewModel _mainViewModel;

    [ObservableProperty]
    private bool _current = false;

    public WordViewModel(int count, MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;

        for (int i = 0; i < count; i++)
        {
            LetterViewModel letter = new LetterViewModel(this);
            Letters.Add(letter);
        }
    }

    public List<LetterViewModel> Letters { get; set; } = new List<LetterViewModel>();

    public int Length => Letters.Count;

    public int LettersSpecified
    {
        get => Letters.Sum(value => string.IsNullOrWhiteSpace(value.Value) ? 0 : 1);
    }

    public string String => string.Join("", Letters.Select(x => x.Value));

    public bool Guessed => Letters.All(x => x.State == LetterState.Guessed);

    public void NotifyLetterChanged()
    {
        _mainViewModel.NotifyWordChanged();
    }
}