using System;
using System.Windows;
using static System.String;

namespace Wpf.Ui.Controls;

public class Hyperlink : Wpf.Ui.Controls.Button
{
    public static readonly DependencyProperty NavigateUriProperty = DependencyProperty.Register("NavigateUri",
        typeof(string), typeof(Hyperlink), new PropertyMetadata(Empty));

    public string NavigateUri
    {
        get => GetValue(NavigateUriProperty) as string;
        set => SetValue(NavigateUriProperty, value);
    }

    public Hyperlink() => Click += RequestNavigate;

    private void RequestNavigate(object sender, RoutedEventArgs eventArgs)
    {
        if (IsNullOrEmpty(NavigateUri))
            return;
        System.Diagnostics.ProcessStartInfo sInfo = new(new Uri(NavigateUri).AbsoluteUri)
        {
            UseShellExecute = true
        };

        System.Diagnostics.Process.Start(sInfo);
    }
}
