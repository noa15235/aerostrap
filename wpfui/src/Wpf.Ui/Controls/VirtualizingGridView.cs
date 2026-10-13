using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls;

public class VirtualizingGridView : ListView
{
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation),
        typeof(Orientation), typeof(VirtualizingGridView),
        new PropertyMetadata(Orientation.Vertical));

    public static readonly DependencyProperty SpacingModeProperty = DependencyProperty.Register(nameof(SpacingMode),
        typeof(SpacingMode), typeof(VirtualizingGridView),
        new PropertyMetadata(SpacingMode.Uniform));

    public static readonly DependencyProperty StretchItemsProperty = DependencyProperty.Register(nameof(StretchItems),
        typeof(bool), typeof(VirtualizingGridView),
        new PropertyMetadata(false));

    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public SpacingMode SpacingMode
    {
        get => (SpacingMode)GetValue(SpacingModeProperty);
        set => SetValue(SpacingModeProperty, value);
    }

    public bool StretchItems
    {
        get => (bool)GetValue(StretchItemsProperty);
        set => SetValue(StretchItemsProperty, value);
    }

    public VirtualizingGridView()
    {
        InitializeItemsPanel();

        VirtualizingPanel.SetCacheLengthUnit(this, VirtualizationCacheLengthUnit.Page);
        VirtualizingPanel.SetCacheLength(this, new VirtualizationCacheLength(1));
        VirtualizingPanel.SetIsVirtualizingWhenGrouping(this, true);
    }

    protected virtual void InitializeItemsPanel()
    {
        var factory = new FrameworkElementFactory(typeof(VirtualizingWrapPanel));

        factory.SetBinding(VirtualizingWrapPanel.OrientationProperty,
            new Binding { Source = this, Path = new PropertyPath(nameof(Orientation)), Mode = BindingMode.OneWay });
        factory.SetBinding(VirtualizingWrapPanel.SpacingModeProperty,
            new Binding { Source = this, Path = new PropertyPath(nameof(SpacingMode)), Mode = BindingMode.OneWay });
        factory.SetBinding(VirtualizingWrapPanel.StretchItemsProperty,
            new Binding { Source = this, Path = new PropertyPath(nameof(StretchItems)), Mode = BindingMode.OneWay });

        ItemsPanel = new ItemsPanelTemplate(factory);
    }
}
