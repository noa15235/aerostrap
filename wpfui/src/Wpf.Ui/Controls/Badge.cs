using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(Badge), "Badge.bmp")]
public class Badge : System.Windows.Controls.ContentControl, IAppearanceControl
{
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(nameof(Appearance),
        typeof(Common.ControlAppearance), typeof(Badge),
        new PropertyMetadata(Common.ControlAppearance.Primary));

    public Common.ControlAppearance Appearance
    {
        get => (Common.ControlAppearance)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }
}
