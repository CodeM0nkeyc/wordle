namespace Wordle.UI.ViewModel;

public partial class LetterViewModel : ObservableObject
{
    private readonly WordViewModel _owner;

    private string _value = string.Empty;

    [ObservableProperty]
    private LetterState _state;

    public LetterViewModel(WordViewModel owner)
    {
        _owner = owner;
    }

    public string Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }
}
