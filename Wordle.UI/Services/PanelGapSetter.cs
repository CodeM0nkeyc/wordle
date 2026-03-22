namespace Wordle.UI.Services;

public class PanelGapSetter
{
    private static readonly PropertyMetadata _columnGapPropertyMetadata =
        new PropertyMetadata(0, OnGapPropertyChanged);

    public static readonly DependencyProperty ColumnGapProperty =
        DependencyProperty.RegisterAttached(
            "ColumnGap",
            typeof(int),
            typeof(PanelGapSetter),
            _columnGapPropertyMetadata);

    private static void OnGapPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Panel panel)
        {
            return;
        }

        panel.Loaded += OnPanelLoaded;
    }

    private static void OnPanelLoaded(object sender , RoutedEventArgs e)
    {
        if (sender is not Panel panel)
        {
            return;
        }

        for (int i = 0; i < panel.Children.Count - 1; i++)
        {
            if (panel.Children[i] is not FrameworkElement child)
            {
                continue;
            }

            child.Margin = new Thickness(0, 0, GetColumnGap(panel), 0);
        }
    }

    public static int GetColumnGap(DependencyObject target)
    {
        return (int)target.GetValue(ColumnGapProperty);
    }

    public static void SetColumnGap(DependencyObject target, int value)
    {
        target.SetValue(ColumnGapProperty, value);
    }
}