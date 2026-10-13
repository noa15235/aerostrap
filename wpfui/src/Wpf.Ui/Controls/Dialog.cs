#nullable enable

using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using static Wpf.Ui.Controls.Interfaces.IDialogControl;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(Dialog), "Dialog.bmp")]
[TemplatePart(Name = "PART_FooterButtonLeft", Type = typeof(System.Windows.Controls.Primitives.ButtonBase))]
[TemplatePart(Name = "PART_FooterButtonRight", Type = typeof(System.Windows.Controls.Primitives.ButtonBase))]
public class Dialog : System.Windows.Controls.ContentControl, IDialogControl
{
    private TaskCompletionSource<ButtonPressed>? _tcs = null;

    private bool _automaticHide;

    private System.Windows.Controls.Primitives.ButtonBase? _leftFooterButton = null;

    private System.Windows.Controls.Primitives.ButtonBase? _rightFooterButton = null;

    private const string ElementFooterButtonLeft = "PART_FooterButtonLeft";

    private const string ElementFooterButtonRight = "PART_FooterButtonRight";

    #region Static properties

    public static readonly DependencyProperty IsShownProperty = DependencyProperty.Register(nameof(IsShown),
        typeof(bool), typeof(Dialog), new PropertyMetadata(false, OnIsShownChange));

    public static readonly DependencyProperty FooterProperty = DependencyProperty.Register(nameof(Footer),
        typeof(object), typeof(Dialog), new PropertyMetadata(null));

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
        typeof(string), typeof(Dialog), new PropertyMetadata(String.Empty));

    public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message),
        typeof(string), typeof(Dialog), new PropertyMetadata(String.Empty));

    public static readonly DependencyProperty DialogWidthProperty =
        DependencyProperty.Register(nameof(DialogWidth),
            typeof(double), typeof(Dialog), new PropertyMetadata(420.0));

    public static readonly DependencyProperty DialogHeightProperty =
        DependencyProperty.Register(nameof(DialogHeight),
            typeof(double), typeof(Dialog), new PropertyMetadata(200.0));

    public static readonly DependencyProperty ButtonLeftNameProperty = DependencyProperty.Register(
        nameof(ButtonLeftName),
        typeof(string), typeof(Dialog), new PropertyMetadata("Action"));

    public static readonly RoutedEvent ButtonLeftClickEvent = EventManager.RegisterRoutedEvent(
        nameof(ButtonLeftClick), RoutingStrategy.Bubble, typeof(Dialog), typeof(Dialog));

    public static readonly DependencyProperty ButtonRightNameProperty = DependencyProperty.Register(
        nameof(ButtonRightName),
        typeof(string), typeof(Dialog), new PropertyMetadata("Hide"));

    public static readonly DependencyProperty ButtonLeftAppearanceProperty = DependencyProperty.Register(
        nameof(ButtonLeftAppearance),
        typeof(Common.ControlAppearance), typeof(Dialog),
        new PropertyMetadata(Common.ControlAppearance.Primary));

    public static readonly DependencyProperty ButtonLeftVisibilityProperty = DependencyProperty.Register(
        nameof(ButtonLeftVisibility),
        typeof(System.Windows.Visibility), typeof(Dialog),
        new PropertyMetadata(System.Windows.Visibility.Visible));

    public static readonly RoutedEvent ButtonRightClickEvent = EventManager.RegisterRoutedEvent(
        nameof(ButtonRightClick), RoutingStrategy.Bubble, typeof(Dialog), typeof(Dialog));

    public static readonly DependencyProperty ButtonRightAppearanceProperty = DependencyProperty.Register(
        nameof(ButtonRightAppearance),
        typeof(Common.ControlAppearance), typeof(Dialog),
        new PropertyMetadata(Common.ControlAppearance.Secondary));

    public static readonly DependencyProperty ButtonRightVisibilityProperty = DependencyProperty.Register(
        nameof(ButtonRightVisibility),
        typeof(System.Windows.Visibility), typeof(Dialog),
        new PropertyMetadata(System.Windows.Visibility.Visible));

    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(Dialog), new PropertyMetadata(null));

    #endregion Static properties

    public bool IsShown
    {
        get => (bool)GetValue(IsShownProperty);
        protected set => SetValue(IsShownProperty, value);
    }

    public object Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
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

    public double DialogWidth
    {
        get => (int)GetValue(DialogWidthProperty);
        set => SetValue(DialogWidthProperty, value);
    }

    public double DialogHeight
    {
        get => (int)GetValue(DialogHeightProperty);
        set => SetValue(DialogHeightProperty, value);
    }

    public string ButtonLeftName
    {
        get => (string)GetValue(ButtonLeftNameProperty);
        set => SetValue(ButtonLeftNameProperty, value);
    }

    public Common.ControlAppearance ButtonLeftAppearance
    {
        get => (Common.ControlAppearance)GetValue(ButtonLeftAppearanceProperty);
        set => SetValue(ButtonLeftAppearanceProperty, value);
    }

    public System.Windows.Visibility ButtonLeftVisibility
    {
        get => (System.Windows.Visibility)GetValue(ButtonLeftVisibilityProperty);
        set => SetValue(ButtonLeftVisibilityProperty, value);
    }

    public event RoutedEventHandler ButtonLeftClick
    {
        add => AddHandler(ButtonLeftClickEvent, value);
        remove => RemoveHandler(ButtonLeftClickEvent, value);
    }

    public string ButtonRightName
    {
        get => (string)GetValue(ButtonRightNameProperty);
        set => SetValue(ButtonRightNameProperty, value);
    }

    public Common.ControlAppearance ButtonRightAppearance
    {
        get => (Common.ControlAppearance)GetValue(ButtonRightAppearanceProperty);
        set => SetValue(ButtonRightAppearanceProperty, value);
    }

    public System.Windows.Visibility ButtonRightVisibility
    {
        get => (System.Windows.Visibility)GetValue(ButtonRightVisibilityProperty);
        set => SetValue(ButtonRightVisibilityProperty, value);
    }

    public event RoutedEventHandler ButtonRightClick
    {
        add => AddHandler(ButtonRightClickEvent, value);
        remove => RemoveHandler(ButtonRightClickEvent, value);
    }

    public Common.IRelayCommand TemplateButtonCommand =>
        (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent(nameof(Opened),
        RoutingStrategy.Bubble, typeof(RoutedDialogEvent), typeof(Dialog));

    public event RoutedDialogEvent Opened
    {
        add => AddHandler(OpenedEvent, value);
        remove => RemoveHandler(OpenedEvent, value);
    }

    public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent(nameof(Closed),
        RoutingStrategy.Bubble, typeof(RoutedDialogEvent), typeof(Dialog));

    public event RoutedDialogEvent Closed
    {
        add => AddHandler(ClosedEvent, value);
        remove => RemoveHandler(ClosedEvent, value);
    }

    public Dialog()
    {
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => OnTemplateButtonClick(this, o)));
    }

    public Task<ButtonPressed> ShowAndWaitAsync()
    {
        _automaticHide = false;

        Show();

        _tcs = new TaskCompletionSource<ButtonPressed>();

        return _tcs.Task;
    }

    public Task<ButtonPressed> ShowAndWaitAsync(bool hideOnClick)
    {
        _automaticHide = hideOnClick;

        Show();

        _tcs = new TaskCompletionSource<ButtonPressed>();

        return _tcs.Task;
    }

    public Task<ButtonPressed> ShowAndWaitAsync(string title, string message)
    {
        _automaticHide = false;

        if (IsShown)
            Hide();

        Show(title, message);

        _tcs = new TaskCompletionSource<ButtonPressed>();

        return _tcs.Task;
    }

    public Task<ButtonPressed> ShowAndWaitAsync(string title, string message, bool hideOnClick)
    {
        _automaticHide = hideOnClick;

        if (IsShown)
            Hide();

        Show(title, message);

        _tcs = new TaskCompletionSource<ButtonPressed>();

        return _tcs.Task;
    }

    public bool Show()
    {
        if (IsShown)
            return false;

        _automaticHide = false;

        IsShown = true;

        FocusFirstButton();

        return true;
    }

    public bool Show(string title, string message)
    {
        if (IsShown)
            Hide();

        _automaticHide = false;

        Title = title;
        Message = message;
        IsShown = true;

        FocusFirstButton();

        return true;
    }

    public bool Hide()
    {
        if (!IsShown)
            return false;

        IsShown = false;

        return true;
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        if (GetTemplateChild(ElementFooterButtonLeft) is System.Windows.Controls.Primitives.ButtonBase leftButton)
            _leftFooterButton = leftButton;

        if (GetTemplateChild(ElementFooterButtonLeft) is System.Windows.Controls.Primitives.ButtonBase rightButton)
            _rightFooterButton = rightButton;
    }

    protected virtual void OnOpened()
    {
        var newEvent = new RoutedEventArgs(Dialog.OpenedEvent, this);
        RaiseEvent(newEvent);
    }

    protected virtual void OnClosed()
    {
        var newEvent = new RoutedEventArgs(Dialog.ClosedEvent, this);
        RaiseEvent(newEvent);
    }

    protected virtual void OnTemplateButtonClick(object sender, object? parameter)
    {
        if (parameter is not String parameterString)
            return;

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO: {typeof(Dialog)} button clicked with param: {parameterString}",
            "Wpf.Ui.Dialog");
#endif

        switch (parameterString)
        {
            case "left":
                RaiseEvent(new RoutedEventArgs(ButtonLeftClickEvent, this));

                _tcs?.TrySetResult(ButtonPressed.Left);

                break;

            case "right":
                RaiseEvent(new RoutedEventArgs(ButtonRightClickEvent, this));

                _tcs?.TrySetResult(ButtonPressed.Right);

                break;
        }

        if (_automaticHide)
            Hide();
    }

    private static void OnIsShownChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Dialog control)
            return;

        if (control.IsShown)
            control.OnOpened();
        else
            control.OnClosed();
    }

    private void FocusFirstButton()
    {
        if (Footer != null)
            return;

        if (ButtonLeftVisibility == Visibility.Visible)
        {
            if (_leftFooterButton != null)
                _leftFooterButton.Focus();

            return;
        }

        if (ButtonRightVisibility != Visibility.Visible)
            return;

        if (_rightFooterButton != null)
            _rightFooterButton.Focus();
    }
}
