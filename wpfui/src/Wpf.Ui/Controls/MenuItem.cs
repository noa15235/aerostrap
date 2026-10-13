using System.ComponentModel;
using System.Windows;

namespace Wpf.Ui.Controls;

public class MenuItem : System.Windows.Controls.MenuItem
{
    public static readonly DependencyProperty SymbolIconProperty = DependencyProperty.Register(nameof(SymbolIcon),
        typeof(Common.SymbolRegular), typeof(Wpf.Ui.Controls.MenuItem),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    public static readonly DependencyProperty SymbolIconFilledProperty = DependencyProperty.Register(
        nameof(SymbolIconFilled),
        typeof(bool), typeof(Wpf.Ui.Controls.MenuItem), new PropertyMetadata(false));

    [Bindable(true), Category("Appearance")]
    public Common.SymbolRegular SymbolIcon
    {
        get => (Common.SymbolRegular)GetValue(SymbolIconProperty);
        set => SetValue(SymbolIconProperty, value);
    }

    [Bindable(true), Category("Appearance")]
    public bool SymbolIconFilled
    {
        get => (bool)GetValue(SymbolIconFilledProperty);
        set => SetValue(SymbolIconFilledProperty, value);
    }

}
