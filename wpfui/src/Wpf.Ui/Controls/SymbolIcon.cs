using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Wpf.Ui.Extensions;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(SymbolIcon), "SymbolIcon.bmp")]
public class SymbolIcon : System.Windows.Controls.Control
{
    public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(nameof(Symbol),
        typeof(Common.SymbolRegular), typeof(SymbolIcon),
        new PropertyMetadata(Common.SymbolRegular.Empty, OnGlyphChanged));

    public static readonly DependencyProperty RawSymbolProperty = DependencyProperty.Register(nameof(RawSymbol),
        typeof(string), typeof(SymbolIcon), new PropertyMetadata("\uEA01"));

    public static readonly DependencyProperty FilledProperty = DependencyProperty.Register(nameof(Filled),
        typeof(bool), typeof(SymbolIcon), new PropertyMetadata(false, OnGlyphChanged));

    public Common.SymbolRegular Symbol
    {
        get => (Common.SymbolRegular)GetValue(SymbolProperty);
        set => SetValue(SymbolProperty, value);
    }

    public string RawSymbol
    {
        get => (string)GetValue(RawSymbolProperty);
    }

    public bool Filled
    {
        get => (bool)GetValue(FilledProperty);
        set => SetValue(FilledProperty, value);
    }

    private static void OnGlyphChanged(DependencyObject dependency, DependencyPropertyChangedEventArgs eventArgs)
    {
        if (dependency is not SymbolIcon control)
            return;

        if ((bool)control.GetValue(FilledProperty))
            control.SetValue(RawSymbolProperty, control.Symbol.Swap().GetString());
        else
            control.SetValue(RawSymbolProperty, control.Symbol.GetString());
    }
}
