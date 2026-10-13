#nullable enable

using System.ComponentModel;
using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls.Navigation;

public class NavigationBackButton : System.Windows.Controls.Button
{
    public static readonly DependencyProperty NavigationProperty = DependencyProperty.Register(nameof(Navigation),
        typeof(INavigation), typeof(NavigationBackButton), new PropertyMetadata(null));

    [Bindable(true), Category("Behavior")]
    public INavigation? Navigation
    {
        get => (INavigation)GetValue(NavigationProperty);
        set => SetValue(NavigationProperty, value);
    }

    public NavigationBackButton()
    {
        SetValue(CommandProperty, new Common.RelayCommand(_ => Navigation?.NavigateBack(), () => Navigation is not null && Navigation.CanGoBack));
    }
}
