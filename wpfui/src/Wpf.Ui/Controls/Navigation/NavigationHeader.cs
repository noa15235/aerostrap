using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Wpf.Ui.Controls.Interfaces;
using Brush = System.Windows.Media.Brush;
using SystemColors = System.Windows.SystemColors;

namespace Wpf.Ui.Controls.Navigation;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(NavigationHeader), "NavigationHeader.bmp")]
public class NavigationHeader : System.Windows.Controls.Control, INavigationControl, IIconControl
{
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
        typeof(string), typeof(NavigationHeader),
        new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(Common.SymbolRegular), typeof(NavigationHeader),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
        typeof(bool), typeof(NavigationHeader), new PropertyMetadata(false));

    public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(nameof(IconForeground),
        typeof(Brush), typeof(NavigationHeader), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize),
        typeof(double), typeof(NavigationHeader), new FrameworkPropertyMetadata(13d));

    [Bindable(true)]
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    [Bindable(true), Category("Appearance")]
    public Common.SymbolRegular Icon
    {
        get => (Common.SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    [Bindable(true), Category("Appearance")]
    public bool IconFilled
    {
        get => (bool)GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
    }

    [Bindable(true), Category("Appearance")]
    public Brush IconForeground
    {
        get => (Brush)GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    [Bindable(true), Category("Appearance")]
    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }
}
