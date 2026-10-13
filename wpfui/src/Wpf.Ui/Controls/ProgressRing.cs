using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls.Primitives;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(ProgressRing), "ProgressRing.bmp")]
public class ProgressRing : RangeBase
{
    public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(
        nameof(IsIndeterminate),
        typeof(bool), typeof(ProgressRing),
        new PropertyMetadata(false));

    public static readonly DependencyProperty EngAngleProperty = DependencyProperty.Register(nameof(EngAngle),
        typeof(double), typeof(ProgressRing),
        new PropertyMetadata(180.0d));

    public static readonly DependencyProperty IndeterminateAngleProperty = DependencyProperty.Register(
        nameof(IndeterminateAngle),
        typeof(double), typeof(ProgressRing),
        new PropertyMetadata(180.0d));

    public static readonly DependencyProperty CoverRingStrokeProperty =
        DependencyProperty.RegisterAttached(
            nameof(CoverRingStroke),
            typeof(Brush),
            typeof(ProgressRing),
            new FrameworkPropertyMetadata(
                Brushes.Black,
                FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender |
                FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty CoverRingVisibilityProperty = DependencyProperty.Register(
        nameof(CoverRingVisibility),
        typeof(System.Windows.Visibility), typeof(ProgressRing),
        new PropertyMetadata(System.Windows.Visibility.Visible));

    public bool IsIndeterminate
    {
        get => (bool)GetValue(IsIndeterminateProperty);
        set => SetValue(IsIndeterminateProperty, value);
    }

    public double EngAngle
    {
        get => (double)GetValue(EngAngleProperty);
        set => SetValue(EngAngleProperty, value);
    }

    public double IndeterminateAngle
    {
        get => (double)GetValue(IndeterminateAngleProperty);
        internal set => SetValue(IndeterminateAngleProperty, value);
    }

    public Brush CoverRingStroke
    {
        get => (Brush)GetValue(CoverRingStrokeProperty);
        internal set => SetValue(CoverRingStrokeProperty, value);
    }

    public System.Windows.Visibility CoverRingVisibility
    {
        get => (System.Windows.Visibility)GetValue(CoverRingVisibilityProperty);
        internal set => SetValue(CoverRingVisibilityProperty, value);
    }

    public ProgressRing()
    {
        UpdateProgressAngle();
    }

    protected void UpdateProgressAngle()
    {
        var endAngle = (360d / Maximum) * Value;

        if (endAngle >= 360)
            endAngle = 359;

        EngAngle = endAngle;
    }

    protected override void OnValueChanged(double oldValue, double newValue)
    {
        base.OnValueChanged(oldValue, newValue);
        UpdateProgressAngle();
    }

    protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
    {
        base.OnMaximumChanged(oldMaximum, newMaximum);
        UpdateProgressAngle();
    }
}
