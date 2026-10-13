using System.ComponentModel;
using System.Drawing;
using System.Windows;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(FontIcon), "FontIcon.bmp")]
public class FontIcon : System.Windows.Controls.Control
{
    public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
        nameof(Glyph),
        typeof(char),
        typeof(FontIcon),
        new PropertyMetadata('\uE006')
    );

    public char Glyph
    {
        get => (char)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }
}
