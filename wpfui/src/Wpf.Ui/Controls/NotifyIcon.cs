using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Common;
using Wpf.Ui.Services.Internal;
using Wpf.Ui.Tray;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(NotifyIcon), "NotifyIcon.bmp")]
public class NotifyIcon : System.Windows.FrameworkElement
{
    private readonly NotifyIconService _notifyIconService;

    protected bool Disposed = false;

    #region Public variables

    public int Id => _notifyIconService.Id;

    public bool IsRegistered => _notifyIconService.IsRegistered;

    public HwndSource HookWindow { get; set; }

    public IntPtr ParentHandle { get; set; }

    #endregion

    #region Properties

    public static readonly DependencyProperty TooltipTextProperty = DependencyProperty.Register(nameof(TooltipText),
        typeof(string), typeof(NotifyIcon),
        new PropertyMetadata(String.Empty, OnTooltipTextChanged));

    public static readonly DependencyProperty FocusOnLeftClickProperty = DependencyProperty.Register(
        nameof(FocusOnLeftClick),
        typeof(bool), typeof(NotifyIcon),
        new PropertyMetadata(true, OnFocusOnLeftClickChanged));

    public static readonly DependencyProperty MenuOnRightClickProperty = DependencyProperty.Register(
        nameof(MenuOnRightClick),
        typeof(bool), typeof(NotifyIcon),
        new PropertyMetadata(true, OnMenuOnRightClickChanged));

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(ImageSource), typeof(NotifyIcon),
        new PropertyMetadata((ImageSource)null!, OnIconChanged));

    public static readonly DependencyProperty MenuProperty = DependencyProperty.Register(nameof(Menu),
        typeof(ContextMenu), typeof(NotifyIcon),
        new PropertyMetadata(null, OnMenuChanged));

    public static readonly DependencyProperty MenuFontSizeProperty = DependencyProperty.Register(
        nameof(MenuFontSize),
        typeof(double), typeof(NotifyIcon),
        new PropertyMetadata(14d));

    public string TooltipText
    {
        get => (string)GetValue(TooltipTextProperty);
        set => SetValue(TooltipTextProperty, value);
    }

    public bool MenuOnRightClick
    {
        get => (bool)GetValue(MenuOnRightClickProperty);
        set => SetValue(MenuOnRightClickProperty, value);
    }

    public bool FocusOnLeftClick
    {
        get => (bool)GetValue(FocusOnLeftClickProperty);
        set => SetValue(FocusOnLeftClickProperty, value);
    }

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public ContextMenu Menu
    {
        get => (ContextMenu)GetValue(MenuProperty);
        set => SetValue(MenuProperty, value);
    }

    public double MenuFontSize
    {
        get => (double)GetValue(MenuFontSizeProperty);
        set => SetValue(MenuFontSizeProperty, value);
    }

    #endregion

    #region Events

    public static readonly RoutedEvent LeftClickEvent =
        EventManager.RegisterRoutedEvent(nameof(LeftClick), RoutingStrategy.Bubble,
            typeof(RoutedNotifyIconEvent), typeof(NotifyIcon));

    public static readonly RoutedEvent LeftDoubleClickEvent =
        EventManager.RegisterRoutedEvent(nameof(LeftDoubleClick), RoutingStrategy.Bubble,
            typeof(RoutedNotifyIconEvent), typeof(NotifyIcon));

    public static readonly RoutedEvent RightClickEvent =
        EventManager.RegisterRoutedEvent(nameof(RightClick), RoutingStrategy.Bubble,
            typeof(RoutedNotifyIconEvent), typeof(NotifyIcon));

    public static readonly RoutedEvent RightDoubleClickEvent =
        EventManager.RegisterRoutedEvent(nameof(RightDoubleClick), RoutingStrategy.Bubble,
            typeof(RoutedNotifyIconEvent), typeof(NotifyIcon));

    public static readonly RoutedEvent MiddleClickEvent =
        EventManager.RegisterRoutedEvent(nameof(MiddleClick), RoutingStrategy.Bubble,
            typeof(RoutedNotifyIconEvent), typeof(NotifyIcon));

    public static readonly RoutedEvent MiddleDoubleClickEvent =
        EventManager.RegisterRoutedEvent(nameof(MiddleDoubleClick), RoutingStrategy.Bubble,
            typeof(RoutedNotifyIconEvent), typeof(NotifyIcon));

    public event RoutedNotifyIconEvent LeftClick
    {
        add => AddHandler(LeftClickEvent, value);
        remove => RemoveHandler(LeftClickEvent, value);
    }

    public event RoutedNotifyIconEvent LeftDoubleClick
    {
        add => AddHandler(LeftDoubleClickEvent, value);
        remove => RemoveHandler(LeftDoubleClickEvent, value);
    }

    public event RoutedNotifyIconEvent RightClick
    {
        add => AddHandler(RightClickEvent, value);
        remove => RemoveHandler(RightClickEvent, value);
    }

    public event RoutedNotifyIconEvent RightDoubleClick
    {
        add => AddHandler(RightDoubleClickEvent, value);
        remove => RemoveHandler(RightDoubleClickEvent, value);
    }

    public event RoutedNotifyIconEvent MiddleClick
    {
        add => AddHandler(MiddleClickEvent, value);
        remove => RemoveHandler(MiddleClickEvent, value);
    }

    public event RoutedNotifyIconEvent MiddleDoubleClick
    {
        add => AddHandler(MiddleDoubleClickEvent, value);
        remove => RemoveHandler(MiddleDoubleClickEvent, value);
    }

    #endregion

    #region General methods

    public NotifyIcon()
    {
        _notifyIconService = new NotifyIconService();

        RegisterHandlers();
    }

    ~NotifyIcon()
        => Dispose(false);

    public void Register()
        => _notifyIconService.Register();

    public void Unregister()
        => _notifyIconService.Unregister();

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    #endregion

    #region Protected methods

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        if (_notifyIconService.IsRegistered)
            return;

        InitializeIcon();

        Register();
    }

    protected virtual void OnLeftClick()
    {
        var newEvent = new RoutedEventArgs(LeftClickEvent, this);
        RaiseEvent(newEvent);
    }

    protected virtual void OnLeftDoubleClick()
    {
        var newEvent = new RoutedEventArgs(LeftDoubleClickEvent, this);
        RaiseEvent(newEvent);
    }

    protected virtual void OnRightClick()
    {
        var newEvent = new RoutedEventArgs(RightClickEvent, this);
        RaiseEvent(newEvent);
    }

    protected virtual void OnRightDoubleClick()
    {
        var newEvent = new RoutedEventArgs(RightDoubleClickEvent, this);
        RaiseEvent(newEvent);
    }

    protected virtual void OnMiddleClick()
    {
        var newEvent = new RoutedEventArgs(MiddleClickEvent, this);
        RaiseEvent(newEvent);
    }

    protected virtual void OnMiddleDoubleClick()
    {
        var newEvent = new RoutedEventArgs(MiddleDoubleClickEvent, this);
        RaiseEvent(newEvent);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (Disposed)
            return;

        Disposed = true;

        if (!disposing)
            return;

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(NotifyIcon)} disposed.", "Wpf.Ui.NotifyIcon");
#endif

        Unregister();

        _notifyIconService.Dispose();
    }

    #endregion

    protected virtual void OnMenuChanged(ContextMenu contextMenu)
    {
        _notifyIconService.ContextMenu = contextMenu;
        _notifyIconService.ContextMenu.FontSize = MenuFontSize;
    }

    private static void OnTooltipTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NotifyIcon notifyIcon)
            return;

        notifyIcon.TooltipText = e.NewValue as string ?? String.Empty;
    }

    private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NotifyIcon notifyIcon)
            return;

        notifyIcon.Icon = e.NewValue as ImageSource;
    }

    private static void OnFocusOnLeftClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NotifyIcon notifyIcon)
            return;

        if (e.NewValue is not bool newValue)
        {
            notifyIcon.FocusOnLeftClick = false;

            return;
        }

        notifyIcon.FocusOnLeftClick = newValue;
    }

    private static void OnMenuOnRightClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NotifyIcon notifyIcon)
            return;

        if (e.NewValue is not bool newValue)
        {
            notifyIcon.MenuOnRightClick = false;

            return;
        }

        notifyIcon.MenuOnRightClick = newValue;
    }

    private static void OnMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not NotifyIcon notifyIcon)
            return;

        if (e.NewValue is not ContextMenu contextMenu)
            return;

        notifyIcon.OnMenuChanged(contextMenu);
    }

    private void InitializeIcon()
    {
        _notifyIconService.TooltipText = TooltipText;
        _notifyIconService.Icon = Icon;
        _notifyIconService.MenuOnRightClick = MenuOnRightClick;
        _notifyIconService.FocusOnLeftClick = FocusOnLeftClick;
    }

    private void RegisterHandlers()
    {
        _notifyIconService.LeftClick += OnLeftClick;
        _notifyIconService.LeftDoubleClick += OnLeftDoubleClick;
        _notifyIconService.RightClick += OnRightClick;
        _notifyIconService.RightDoubleClick += OnRightDoubleClick;
        _notifyIconService.MiddleClick += OnMiddleClick;
        _notifyIconService.MiddleDoubleClick += OnMiddleDoubleClick;
    }
}
