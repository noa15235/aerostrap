using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(VirtualizingItemsControl), "VirtualizingItemsControl.bmp")]
public class VirtualizingItemsControl : System.Windows.Controls.ItemsControl
{
    public static readonly DependencyProperty CacheLengthUnitProperty =
        DependencyProperty.Register(nameof(CacheLengthUnit), typeof(VirtualizationCacheLengthUnit), typeof(VirtualizingItemsControl),
            new FrameworkPropertyMetadata(VirtualizationCacheLengthUnit.Page));

    public VirtualizationCacheLengthUnit CacheLengthUnit
    {
        get => VirtualizingPanel.GetCacheLengthUnit(this);
        set
        {
            SetValue(CacheLengthUnitProperty, value);
            VirtualizingPanel.SetCacheLengthUnit(this, value);
        }
    }

    public VirtualizingItemsControl()
    {
        VirtualizingPanel.SetCacheLengthUnit(this, CacheLengthUnit);
        VirtualizingPanel.SetCacheLength(this, new VirtualizationCacheLength(1));
        VirtualizingPanel.SetIsVirtualizingWhenGrouping(this, true);
    }
}

