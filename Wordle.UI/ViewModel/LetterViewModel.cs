namespace Wordle.UI.ViewModel;

public partial class LetterViewModel : ObservableObject
{
    private readonly WordViewModel _owner;

    [ObservableProperty]
    private string _value = string.Empty;

    [ObservableProperty]
    private LetterState _state;

    public LetterViewModel(WordViewModel owner)
    {
        _owner = owner;
    }

    partial void OnValueChanged(string? oldValue, string newValue)
    {
        _owner.NotifyLetterChanged();
    }
}
