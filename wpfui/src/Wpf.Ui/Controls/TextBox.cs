using System;
using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls.Interfaces;
using Brush = System.Windows.Media.Brush;
using SystemColors = System.Windows.SystemColors;

namespace Wpf.Ui.Controls;

public class TextBox : System.Windows.Controls.TextBox, IIconControl
{
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(Common.SymbolRegular), typeof(TextBox),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register(
        nameof(IconPlacement),
        typeof(Common.ElementPlacement), typeof(TextBox),
        new PropertyMetadata(Common.ElementPlacement.Left));

    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
        typeof(bool), typeof(TextBox), new PropertyMetadata(false));

    public static readonly DependencyProperty IconForegroundProperty =
        DependencyProperty.RegisterAttached(
            nameof(IconForeground),
            typeof(Brush),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                SystemColors.ControlTextBrush,
                FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(nameof(PlaceholderText),
        typeof(string), typeof(TextBox), new PropertyMetadata(String.Empty));

    public static readonly DependencyProperty PlaceholderEnabledProperty = DependencyProperty.Register(
        nameof(PlaceholderEnabled),
        typeof(bool), typeof(TextBox), new PropertyMetadata(true));

    public static readonly DependencyProperty ClearButtonEnabledProperty = DependencyProperty.Register(nameof(ClearButtonEnabled),
        typeof(bool), typeof(TextBox), new PropertyMetadata(true));

    public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(nameof(ShowClearButton),
        typeof(bool), typeof(TextBox), new PropertyMetadata(false));

    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(TextBox), new PropertyMetadata(null));

    public Common.SymbolRegular Icon
    {
        get => (Common.SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public Common.ElementPlacement IconPlacement
    {
        get => (Common.ElementPlacement)GetValue(IconPlacementProperty);
        set => SetValue(IconPlacementProperty, value);
    }

    public bool IconFilled
    {
        get => (bool)GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
    }

    public Brush IconForeground
    {
        get => (Brush)GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public bool PlaceholderEnabled
    {
        get => (bool)GetValue(PlaceholderEnabledProperty);
        set => SetValue(PlaceholderEnabledProperty, value);
    }

    public bool ClearButtonEnabled
    {
        get => (bool)GetValue(ClearButtonEnabledProperty);
        set => SetValue(ClearButtonEnabledProperty, value);
    }

    public bool ShowClearButton
    {
        get => (bool)GetValue(ShowClearButtonProperty);
        protected set => SetValue(ShowClearButtonProperty, value);
    }

    public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    public TextBox()
    {
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => OnTemplateButtonClick(this, o)));
    }

    protected override void OnTextChanged(TextChangedEventArgs e)
    {
        base.OnTextChanged(e);

        if (PlaceholderEnabled && Text.Length > 0)
            PlaceholderEnabled = false;

        if (!PlaceholderEnabled && Text.Length < 1)
            PlaceholderEnabled = true;

        RevealClearButton();
    }

    protected override void OnGotFocus(RoutedEventArgs e)
    {
        base.OnGotFocus(e);

        RevealClearButton();
    }

    protected override void OnLostFocus(RoutedEventArgs e)
    {
        base.OnLostFocus(e);

        HideClearButton();
    }

    protected void RevealClearButton()
    {
        if (ClearButtonEnabled && IsKeyboardFocusWithin)
            ShowClearButton = Text.Length > 0;
    }

    protected void HideClearButton()
    {
        if (ClearButtonEnabled && !IsKeyboardFocusWithin && ShowClearButton)
            ShowClearButton = false;
    }

    protected virtual void OnTemplateButtonClick(object sender, object parameter)
    {
        if (parameter == null)
            return;

        var param = parameter as string ?? String.Empty;

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO: {typeof(TextBox)} button clicked with param: {param}", "Wpf.Ui.TextBox");
#endif

        switch (param)
        {
            case "clear":
                if (Text.Length > 0)
                    Text = String.Empty;

                break;
        }
    }
}
