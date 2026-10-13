using System;
using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls;

public class Breadcrumb : System.Windows.Controls.Control
{
    public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register(nameof(Current),
        typeof(string), typeof(Breadcrumb), new PropertyMetadata(String.Empty));

    public static readonly DependencyProperty NavigationProperty = DependencyProperty.Register(nameof(Navigation),
        typeof(INavigation), typeof(Breadcrumb),
        new PropertyMetadata(null, OnNavigationChanged));

    public string Current
    {
        get => (string)GetValue(CurrentProperty);
        set => SetValue(CurrentProperty, value);
    }

    public INavigation Navigation
    {
        get => GetValue(NavigationProperty) as INavigation;
        set => SetValue(NavigationProperty, value);
    }

    protected virtual void OnNavigated(INavigation sender, RoutedNavigationEventArgs e)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Breadcrumb)} builded, current nav: {Navigation.GetType()}", "Wpf.Ui.Breadcrumb");
#endif

        if (Navigation?.Current is not INavigationItem item)
            return;

        var pageName = item.Content as string;

        if (String.IsNullOrEmpty(pageName))
            return;

        Current = pageName;
    }

    protected virtual void OnNavigationChanged()
    {
        Navigation.Navigated += OnNavigated;
    }

    private static void OnNavigationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Breadcrumb breadcrumb)
            return;

        breadcrumb.OnNavigationChanged();
    }
}
