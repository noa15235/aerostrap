using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(Anchor), "Anchor.bmp")]
public class Anchor : Wpf.Ui.Controls.Button
{
    public static readonly DependencyProperty NavigateUriProperty =
        DependencyProperty.Register(nameof(NavigateUri), typeof(string), typeof(Anchor),
            new PropertyMetadata(String.Empty));

    public string NavigateUri
    {
        get => (string)GetValue(NavigateUriProperty);
        set => SetValue(NavigateUriProperty, value);
    }

    protected override void OnClick()
    {
        RoutedEventArgs newEvent = new RoutedEventArgs(ButtonBase.ClickEvent, this);
        RaiseEvent(newEvent);

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | Anchor clicked, with href: {NavigateUri}", "Wpf.Ui.Anchor");
#endif
        if (String.IsNullOrEmpty(NavigateUri))
            return;
        System.Diagnostics.ProcessStartInfo sInfo = new(new Uri(NavigateUri).AbsoluteUri)
        {
            UseShellExecute = true
        };

        System.Diagnostics.Process.Start(sInfo);
    }
}
