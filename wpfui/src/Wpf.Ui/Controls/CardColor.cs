using System;
using System.Windows;
using System.Windows.Media;

namespace Wpf.Ui.Controls;

public class CardColor : System.Windows.Controls.Control
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
        typeof(string), typeof(CardColor), new PropertyMetadata(String.Empty));

    public static readonly DependencyProperty SubtitleProperty = DependencyProperty.Register(nameof(Subtitle),
        typeof(string), typeof(CardColor), new PropertyMetadata(String.Empty, OnSubtitlePropertyChanged));

    public static readonly DependencyProperty SubtitleFontSizeProperty = DependencyProperty.Register(nameof(SubtitleFontSize),
        typeof(double), typeof(CardColor), new PropertyMetadata(11.0d));

    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color),
        typeof(Color), typeof(CardColor), new PropertyMetadata(Color.FromArgb(0, 0, 0, 0), OnColorPropertyChanged));

    public static readonly DependencyProperty BrushProperty = DependencyProperty.Register(nameof(Brush),
        typeof(Brush), typeof(CardColor),
        new PropertyMetadata(new SolidColorBrush { Color = Color.FromArgb(0, 0, 0, 0) }, OnBrushPropertyChanged));

    public static readonly DependencyProperty CardBrushProperty = DependencyProperty.Register(nameof(CardBrush),
        typeof(Brush), typeof(CardColor),
        new PropertyMetadata(new SolidColorBrush { Color = Color.FromArgb(0, 0, 0, 0) }));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public double SubtitleFontSize
    {
        get => (double)GetValue(SubtitleFontSizeProperty);
        set => SetValue(SubtitleFontSizeProperty, value);
    }

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public Brush Brush
    {
        get => (Brush)GetValue(BrushProperty);
        set => SetValue(BrushProperty, value);
    }

    public Brush CardBrush
    {
        get => (Brush)GetValue(CardBrushProperty);
        internal set => SetValue(CardBrushProperty, value);
    }

    protected virtual void OnSubtitlePropertyChanged()
    {
    }

    protected virtual void OnColorPropertyChanged()
    {
        CardBrush = new SolidColorBrush(Color);
    }

    protected virtual void OnBrushPropertyChanged()
    {
        CardBrush = Brush;
    }

    private static void OnSubtitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not CardColor cardColor)
            return;

        cardColor.OnSubtitlePropertyChanged();
    }

    private static void OnColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not CardColor cardColor)
            return;

        cardColor.OnColorPropertyChanged();
    }

    private static void OnBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not CardColor cardColor)
            return;

        cardColor.OnBrushPropertyChanged();
    }
}
