using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Wpf.Ui.Controls;

public class TreeGrid : System.Windows.Controls.Primitives.Selector
{
    public static readonly DependencyProperty HeadersProperty = DependencyProperty.Register(nameof(Headers),
        typeof(ObservableCollection<TreeGridHeader>), typeof(TreeGrid),
        new PropertyMetadata(new ObservableCollection<TreeGridHeader> { }, OnHeadersChanged));

    [Bindable(true)]
    public ObservableCollection<TreeGridHeader> Headers
    {
        get => GetValue(HeadersProperty) as ObservableCollection<TreeGridHeader>;
        set => SetValue(HeadersProperty, value);
    }

    public TreeGrid()
    {
        var x = new System.Windows.Controls.ContentControl();
        var y = new System.Windows.Controls.ItemsControl();
        var z = new System.Windows.Controls.ListBox();
    }

    protected virtual void OnHeadersChanged()
    {
    }

    protected virtual void OnContentChanged()
    {
    }

    private static void OnHeadersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TreeGrid treeGrid)
            return;

        treeGrid.OnHeadersChanged();
    }

    private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TreeGrid treeGrid)
            return;

        treeGrid.OnContentChanged();
    }
}
