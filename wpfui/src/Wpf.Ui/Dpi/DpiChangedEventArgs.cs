using System.Windows;

namespace Wpf.Ui.Dpi;

public sealed class DpiChangedEventArgs : RoutedEventArgs
{
    public Dpi NewDpi { get; }

    public Dpi OldDpi { get; }
}
