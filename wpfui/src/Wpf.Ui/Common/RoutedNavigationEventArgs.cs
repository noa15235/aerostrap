using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Common;

public class RoutedNavigationEventArgs : RoutedEventArgs
{
    public INavigationItem CurrentPage { get; set; }

    public RoutedNavigationEventArgs(RoutedEvent routedEvent, object source, INavigationItem currentPage) : base(
        routedEvent, source)
    {
        CurrentPage = currentPage;
    }
}
