using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Brush = System.Windows.Media.Brush;
using SystemColors = System.Windows.SystemColors;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(Snackbar), "Snackbar.bmp")]
public class Snackbar : System.Windows.Controls.ContentControl, ISnackbarControl, IIconControl, IAppearanceControl
{
    private readonly EventIdentifier _eventIdentifier;

    public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register(nameof(IsShown),
        typeof(bool), typeof(Snackbar), new PropertyMetadata(false));

    public static readonly DependencyProperty TimeoutProperty = DependencyProperty.Register(nameof(Timeout),
        typeof(int), typeof(Snackbar), new PropertyMetadata(2000));

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(Common.SymbolRegular), typeof(Snackbar),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
        typeof(bool), typeof(Snackbar), new PropertyMetadata(false));

    public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(
        nameof(IconForeground),
        typeof(Brush), typeof(Snackbar), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
        typeof(string), typeof(Snackbar), new PropertyMetadata(String.Empty));

    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message),
        typeof(string), typeof(Snackbar), new PropertyMetadata(String.Empty));

    public static readonly DependencyProperty MessageForegroundProperty = DependencyProperty.Register(
        nameof(MessageForeground),
        typeof(Brush), typeof(Snackbar), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(nameof(Appearance),
        typeof(Common.ControlAppearance), typeof(Snackbar),
        new PropertyMetadata(Common.ControlAppearance.Secondary));

    public static readonly DependencyProperty CloseButtonEnabledProperty = DependencyProperty.Register(
        nameof(CloseButtonEnabled),
        typeof(bool), typeof(Snackbar), new PropertyMetadata(true));

    public static readonly DependencyProperty SlideTransformProperty = DependencyProperty.Register(
        nameof(SlideTransform),
        typeof(TranslateTransform), typeof(Snackbar), new PropertyMetadata(new TranslateTransform()));

    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(Snackbar), new PropertyMetadata(null));

    public bool IsShown
    {
        get => (bool)GetValue(IsShownProperty);
        protected set => SetValue(IsShownProperty, value);
    }

    public int Timeout
    {
        get => (int)GetValue(TimeoutProperty);
        set => SetValue(TimeoutProperty, value);
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

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    [Bindable(true), Category("Appearance")]
    public Brush MessageForeground
    {
        get => (Brush)GetValue(MessageForegroundProperty);
        set => SetValue(MessageForegroundProperty, value);
    }

    [Bindable(true), Category("Appearance")]
    public Common.ControlAppearance Appearance
    {
        get => (Common.ControlAppearance)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    public bool CloseButtonEnabled
    {
        get => (bool)GetValue(CloseButtonEnabledProperty);
        set => SetValue(CloseButtonEnabledProperty, value);
    }

    public TranslateTransform SlideTransform
    {
        get => (TranslateTransform)GetValue(SlideTransformProperty);
        set => SetValue(SlideTransformProperty, value);
    }

    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(nameof(Opened),
        RoutingStrategy.Bubble, typeof(RoutedSnackbarEvent), typeof(Snackbar));

    public event RoutedSnackbarEvent Opened
    {
        add => AddHandler(OpenedEvent, value);
        remove => RemoveHandler(OpenedEvent, value);
    }

    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(nameof(Closed),
        RoutingStrategy.Bubble, typeof(RoutedSnackbarEvent), typeof(Snackbar));

    public event RoutedSnackbarEvent Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    public Snackbar()
    {
        _eventIdentifier = new EventIdentifier();

        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => OnTemplateButtonClick(this, o)));
    }

    public bool Show()
    {
#pragma warning disable CS4014
        ShowComponentAsync();
#pragma warning restore CS4014

        return true;
    }

    public bool Show(string title)
    {
#pragma warning disable CS4014
        ShowComponentAsync(title);
#pragma warning restore CS4014

        return true;
    }

    public bool Show(string title, string message)
    {
#pragma warning disable CS4014
        ShowComponentAsync(title, message);
#pragma warning restore CS4014

        return true;
    }

    public bool Show(string title, string message, SymbolRegular icon)
    {
#pragma warning disable CS4014
        ShowComponentAsync(title, message, icon);
#pragma warning restore CS4014

        return true;
    }

    public bool Show(string title, string message, SymbolRegular icon, ControlAppearance appearance)
    {
#pragma warning disable CS4014
        ShowComponentAsync(title, message, icon, appearance);
#pragma warning restore CS4014

        return true;
    }

    public async Task<bool> ShowAsync()
        => await ShowComponentAsync();

    public async Task<bool> ShowAsync(string title)
        => await ShowComponentAsync(title);

    public async Task<bool> ShowAsync(string title, string message)
        => await ShowComponentAsync(title, message);

    public async Task<bool> ShowAsync(string title, string message, SymbolRegular icon)
        => await ShowComponentAsync(title, message, icon);

    public async Task<bool> ShowAsync(string title, string message, SymbolRegular icon, ControlAppearance appearance)
        => await ShowComponentAsync(title, message, icon, appearance);

    public bool Hide()
    {
#pragma warning disable CS4014
        HideComponentAsync(0);
#pragma warning restore CS4014

        return true;
    }

    public async Task<bool> HideAsync()
    {
        return await HideComponentAsync(0);
    }

    protected virtual async void OnTemplateButtonClick(object sender, object parameter)
    {
        if (parameter is not String parameterString)
            return;

        if (parameterString == "close")
            await HideAsync();
    }

    protected virtual void OnOpened()
    {
        RaiseEvent(new RoutedEventArgs(
            Snackbar.OpenedEvent,
            this));
    }

    protected virtual void OnClosed()
    {
        RaiseEvent(new RoutedEventArgs(
            Snackbar.ClosedEvent,
            this));
    }

    private async Task<bool> ShowComponentAsync()
    {
        await HideIfVisible();

        IsShown = true;

        OnOpened();

        if (Timeout > 0)
            await HideComponentAsync(Timeout);

        return true;
    }

    private async Task<bool> ShowComponentAsync(string title)
    {
        await HideIfVisible();

        Title = title;
        IsShown = true;

        OnOpened();

        if (Timeout > 0)
            await HideComponentAsync(Timeout);

        return true;
    }

    private async Task<bool> ShowComponentAsync(string title, string message)
    {
        await HideIfVisible();

        Title = title;
        Message = message;
        IsShown = true;

        OnOpened();

        if (Timeout > 0)
            await HideComponentAsync(Timeout);

        return true;
    }

    private async Task<bool> ShowComponentAsync(string title, string message, SymbolRegular icon)
    {
        await HideIfVisible();

        Title = title;
        Message = message;
        Icon = icon;

        IsShown = true;

        OnOpened();

        if (Timeout > 0)
            await HideComponentAsync(Timeout);

        return true;
    }

    private async Task<bool> ShowComponentAsync(string title, string message, SymbolRegular icon, ControlAppearance appearance)
    {
        await HideIfVisible();

        Title = title;
        Message = message;
        Icon = icon;
        Appearance = appearance;

        IsShown = true;

        OnOpened();

        if (Timeout > 0)
            await HideComponentAsync(Timeout);

        return true;
    }

    private async Task<bool> HideComponentAsync(int timeout)
    {
        if (!IsShown)
            return false;

        if (timeout < 1)
            IsShown = false;

        var currentEvent = _eventIdentifier.GetNext();

        await Task.Delay(timeout);

        if (Application.Current == null)
            return false;

        if (!_eventIdentifier.IsEqual(currentEvent))
            return false;

        IsShown = false;

        OnClosed();

        return true;
    }

    private async Task HideIfVisible()
    {
        if (!IsShown)
            return;

        await HideComponentAsync(0);

        await Task.Delay(300);
    }
}
