using System.ComponentModel;
using System.Drawing;
using System.Windows;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(Card), "Card.bmp")]
public class Card : System.Windows.Controls.ContentControl
{
    public static readonly DependencyProperty FooterProperty =
        DependencyProperty.Register(nameof(Footer), typeof(object), typeof(Card),
            new PropertyMetadata(null, FooterChangedCallback));

    public static readonly DependencyProperty HasFooterProperty =
        DependencyProperty.Register(nameof(HasFooter), typeof(bool), typeof(Card),
            new PropertyMetadata(false));

    public object Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public bool HasFooter
    {
        get => (bool)GetValue(HasFooterProperty);
        internal set => SetValue(HasFooterProperty, value);
    }

    private static void FooterChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Card control)
            return;

        control.SetValue(HasFooterProperty, control.Footer != null);
    }
}
