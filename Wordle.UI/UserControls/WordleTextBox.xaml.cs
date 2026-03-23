namespace Wordle.UI.UserControls;

public partial class WordleTextBox : UserControl
{
    private readonly LinkedList<TextBox> _textBoxList = new LinkedList<TextBox>();
    private LinkedListNode<TextBox>? _currentTextBox;

    public static readonly DependencyProperty LettersProperty =
        DependencyProperty.Register(
            "Letters",
            typeof(IEnumerable<LetterViewModel>),
            typeof(WordleTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public WordleTextBox()
    {
        InitializeComponent();
        IsEnabledChanged += OnEnabledPropertyChanged;
    }

    private async void OnEnabledPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if ((bool)e.NewValue)
        {
            await Task.Delay(100);
            _currentTextBox?.Value.Focus();
        }
    }

    public IEnumerable<LetterViewModel> Letters
    {
        get => (IEnumerable<LetterViewModel>)GetValue(LettersProperty);
        set => SetValue(LettersProperty, value);
    }

    private void SetupTextInputList(object sender, RoutedEventArgs e)
    {
        if (sender is not ListView listView)
        {
            throw new InvalidOperationException($"{nameof(SetupTextInputList)} should be used only on ListView objects.");
        }

        FillTextBoxLinkedList(listView);
        _currentTextBox = _textBoxList.First;

        if (_currentTextBox is not null)
        {
            TextBox current = _currentTextBox.Value;
            current.IsEnabled = true;
            current.Focus();
        }
    }

    private void OnTextChangedMove(object sender, TextChangedEventArgs e)
    {
        if (_currentTextBox?.Next is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(_currentTextBox.Value.Text))
        {
            _currentTextBox.Value.Text = string.Empty;
            return;
        }

        RefocusTextBox(_currentTextBox.Value, _currentTextBox.Next.Value);
        _currentTextBox = _currentTextBox.Next;
    }

    private void OnBackspaceKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Back)
        {
            if (_currentTextBox?.Previous is null ||
                !string.IsNullOrEmpty(_currentTextBox.Value.Text))
            {
                return;
            }

            RefocusTextBox(_currentTextBox.Value, _currentTextBox.Previous.Value);
            _currentTextBox = _currentTextBox.Previous;
        }
    }

    private void FillTextBoxLinkedList(DependencyObject parentElement)
    {
        if (parentElement is TextBox textBox)
        {
            _textBoxList.AddLast(textBox);
            return;
        }

        int childrenCount = VisualTreeHelper.GetChildrenCount(parentElement);

        if (childrenCount == 0)
        {
            return;
        }

        for (int i = 0; i < childrenCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parentElement, i);
            FillTextBoxLinkedList(child);
        }
    }

    private void RefocusTextBox(TextBox current, TextBox next)
    {
        current.IsEnabled = false;
        next.IsEnabled = true;
        next.Focus();
    }
}