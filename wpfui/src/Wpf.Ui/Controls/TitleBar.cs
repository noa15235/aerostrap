using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Dpi;
using Wpf.Ui.TitleBar;

namespace Wpf.Ui.Controls;

[TemplatePart(Name = "PART_MainGrid", Type = typeof(System.Windows.Controls.Grid))]
[TemplatePart(Name = "PART_MaximizeButton", Type = typeof(Wpf.Ui.Controls.Button))]
[TemplatePart(Name = "PART_RestoreButton", Type = typeof(Wpf.Ui.Controls.Button))]
public class TitleBar : System.Windows.Controls.Control, IThemeControl
{
    private const string ElementMainGrid = "PART_MainGrid";

    private const string ElementMaximizeButton = "PART_MaximizeButton";

    private const string ElementRestoreButton = "PART_RestoreButton";

    private System.Windows.Window _parent;

    internal Interop.WinDef.POINT _doubleClickPoint;

    internal SnapLayout _snapLayout;

    public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(nameof(Theme),
        typeof(Appearance.ThemeType), typeof(TitleBar), new PropertyMetadata(Appearance.ThemeType.Unknown));

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title),
        typeof(string), typeof(TitleBar), new PropertyMetadata(null));

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header),
        typeof(object), typeof(TitleBar), new PropertyMetadata(null));

    public static readonly DependencyProperty ButtonsForegroundProperty = DependencyProperty.Register(
        nameof(ButtonsForeground),
        typeof(Brush), typeof(TitleBar), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty ButtonsBackgroundProperty = DependencyProperty.Register(
        nameof(ButtonsBackground),
        typeof(Brush), typeof(TitleBar), new FrameworkPropertyMetadata(SystemColors.ControlBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    public static readonly DependencyProperty MinimizeToTrayProperty = DependencyProperty.Register(
        nameof(MinimizeToTray),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(false));

    public static readonly DependencyProperty UseSnapLayoutProperty = DependencyProperty.Register(
        nameof(UseSnapLayout),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(false));

    public static readonly DependencyProperty IsMaximizedProperty = DependencyProperty.Register(nameof(IsMaximized),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(false));

    public static readonly DependencyProperty ForceShutdownProperty =
        DependencyProperty.Register(nameof(ForceShutdown), typeof(bool), typeof(TitleBar),
            new PropertyMetadata(false));

    public static readonly DependencyProperty ShowMaximizeProperty = DependencyProperty.Register(
        nameof(ShowMaximize),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

    public static readonly DependencyProperty ShowMinimizeProperty = DependencyProperty.Register(
        nameof(ShowMinimize),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

    public static readonly DependencyProperty ShowHelpProperty = DependencyProperty.Register(
        nameof(ShowHelp),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(false));

    public static readonly DependencyProperty ShowCloseProperty = DependencyProperty.Register(
        nameof(ShowClose),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

    public static readonly DependencyProperty CanMaximizeProperty = DependencyProperty.Register(
        nameof(CanMaximize),
        typeof(bool), typeof(TitleBar), new PropertyMetadata(true));

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
        nameof(Icon),
        typeof(ImageSource), typeof(TitleBar), new PropertyMetadata(null));

    public static readonly DependencyProperty TrayProperty = DependencyProperty.Register(
        nameof(Tray),
        typeof(NotifyIcon), typeof(TitleBar), new PropertyMetadata(null));

    public static readonly RoutedEvent CloseClickedEvent = EventManager.RegisterRoutedEvent(
        nameof(CloseClicked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TitleBar));

    public static readonly RoutedEvent MaximizeClickedEvent = EventManager.RegisterRoutedEvent(
        nameof(MaximizeClicked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TitleBar));

    public static readonly RoutedEvent MinimizeClickedEvent = EventManager.RegisterRoutedEvent(
        nameof(MinimizeClicked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TitleBar));

    public static readonly RoutedEvent HelpClickedEvent = EventManager.RegisterRoutedEvent(
        nameof(HelpClicked), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TitleBar));

    public static readonly DependencyProperty TemplateButtonCommandProperty =
        DependencyProperty.Register(nameof(TemplateButtonCommand),
            typeof(Common.IRelayCommand), typeof(TitleBar), new PropertyMetadata(null));

    public Appearance.ThemeType Theme
    {
        get => (Appearance.ThemeType)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public object Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    [Bindable(true), Category("Appearance")]
    public Brush ButtonsForeground
    {
        get => (Brush)GetValue(ButtonsForegroundProperty);
        set => SetValue(ButtonsForegroundProperty, value);
    }

    [Bindable(true), Category("Appearance")]
    public Brush ButtonsBackground
    {
        get => (Brush)GetValue(ButtonsBackgroundProperty);
        set => SetValue(ButtonsBackgroundProperty, value);
    }

    public bool MinimizeToTray
    {
        get => (bool)GetValue(MinimizeToTrayProperty);
        set => SetValue(MinimizeToTrayProperty, value);
    }

    public bool UseSnapLayout
    {
        get => (bool)GetValue(UseSnapLayoutProperty);
        set => SetValue(UseSnapLayoutProperty, value);
    }

    public bool IsMaximized
    {
        get => (bool)GetValue(IsMaximizedProperty);
        internal set => SetValue(IsMaximizedProperty, value);
    }

    public bool ForceShutdown
    {
        get => (bool)GetValue(ForceShutdownProperty);
        set => SetValue(ForceShutdownProperty, value);
    }

    public bool ShowMaximize
    {
        get => (bool)GetValue(ShowMaximizeProperty);
        set => SetValue(ShowMaximizeProperty, value);
    }

    public bool ShowMinimize
    {
        get => (bool)GetValue(ShowMinimizeProperty);
        set => SetValue(ShowMinimizeProperty, value);
    }

    public bool ShowHelp
    {
        get => (bool)GetValue(ShowHelpProperty);
        set => SetValue(ShowHelpProperty, value);
    }

    public bool ShowClose
    {
        get => (bool)GetValue(ShowCloseProperty);
        set => SetValue(ShowCloseProperty, value);
    }

    public bool CanMaximize
    {
        get => (bool)GetValue(CanMaximizeProperty);
        set => SetValue(CanMaximizeProperty, value);
    }

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public NotifyIcon Tray
    {
        get => (NotifyIcon)GetValue(TrayProperty);
        set => SetValue(TrayProperty, value);
    }

    public event RoutedEventHandler CloseClicked
    {
        add => AddHandler(CloseClickedEvent, value);
        remove => RemoveHandler(CloseClickedEvent, value);
    }

    public event RoutedEventHandler MaximizeClicked
    {
        add => AddHandler(MaximizeClickedEvent, value);
        remove => RemoveHandler(MaximizeClickedEvent, value);
    }

    public event RoutedEventHandler MinimizeClicked
    {
        add => AddHandler(MinimizeClickedEvent, value);
        remove => RemoveHandler(MinimizeClickedEvent, value);
    }

    public event RoutedEventHandler HelpClicked
    {
        add => AddHandler(HelpClickedEvent, value);
        remove => RemoveHandler(HelpClickedEvent, value);
    }

    public Common.IRelayCommand TemplateButtonCommand => (Common.IRelayCommand)GetValue(TemplateButtonCommandProperty);

    public Action<TitleBar, System.Windows.Window> MaximizeActionOverride { get; set; } = null;

    public Action<TitleBar, System.Windows.Window> MinimizeActionOverride { get; set; } = null;

    internal System.Windows.Window ParentWindow => _parent ??= System.Windows.Window.GetWindow(this);

    public TitleBar()
    {
        SetValue(TemplateButtonCommandProperty, new Common.RelayCommand(o => OnTemplateButtonClick(this, o)));

        Loaded += OnLoaded;
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        Theme = Appearance.Theme.GetAppTheme();
        Appearance.Theme.Changed += OnThemeChanged;
    }

    protected virtual void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (ParentWindow != null)
            ParentWindow.StateChanged += OnParentWindowStateChanged;
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var mainGrid = GetTemplateChild(ElementMainGrid) as System.Windows.Controls.Grid;
        var maximizeButton = GetTemplateChild(ElementMaximizeButton) as Wpf.Ui.Controls.Button;
        var restoreButton = GetTemplateChild(ElementRestoreButton) as Wpf.Ui.Controls.Button;

        if (mainGrid != null)
        {
            mainGrid.MouseLeftButtonDown += OnMainGridMouseLeftButtonDown;
            mainGrid.MouseMove += OnMainGridMouseMove;
        }

        if (ShowMaximize && UseSnapLayout && maximizeButton != null && restoreButton != null)
            InitializeSnapLayout(maximizeButton, restoreButton);
    }

    protected virtual void OnThemeChanged(Appearance.ThemeType currentTheme, Color systemAccent)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TitleBar)} received theme -  {currentTheme}",
            "Wpf.Ui.TitleBar");
#endif
        Theme = currentTheme;

        if (_snapLayout != null)
            _snapLayout.Theme = currentTheme;
    }

    private void CloseWindow()
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TitleBar)}.CloseWindow:ForceShutdown -  {ForceShutdown}",
            "Wpf.Ui.TitleBar");
#endif

        if (ForceShutdown)
        {
            Application.Current.Shutdown();

            return;
        }

        ParentWindow.Close();
    }

    private void MinimizeWindow()
    {
        if (MinimizeToTray && Tray.IsRegistered && MinimizeWindowToTray())
            return;

        if (MinimizeActionOverride != null)
        {
            MinimizeActionOverride(this, _parent);

            return;
        }

        ParentWindow.WindowState = WindowState.Minimized;
    }

    private void MaximizeWindow()
    {
        if (!CanMaximize)
            return;

        if (MaximizeActionOverride != null)
        {
            MaximizeActionOverride(this, _parent);

            return;
        }

        if (ParentWindow.WindowState == WindowState.Normal)
        {
            IsMaximized = true;
            ParentWindow.WindowState = WindowState.Maximized;
        }
        else
        {
            IsMaximized = false;
            ParentWindow.WindowState = WindowState.Normal;
        }
    }

    private void RestoreWindow()
    {
        if (!CanMaximize)
            return;

        if (MaximizeActionOverride != null)
        {
            MaximizeActionOverride(this, _parent);

            return;
        }

        if (ParentWindow.WindowState == WindowState.Normal)
        {
            IsMaximized = true;
            ParentWindow.WindowState = WindowState.Maximized;
        }
        else
        {
            IsMaximized = false;
            ParentWindow.WindowState = WindowState.Normal;
        }
    }

    private bool MinimizeWindowToTray()
    {
        if (!Tray.IsRegistered)
            return false;

        ParentWindow.WindowState = WindowState.Minimized;
        ParentWindow.Hide();

        return true;
    }

    private void InitializeSnapLayout(Wpf.Ui.Controls.Button maximizeButton, Wpf.Ui.Controls.Button restoreButton)
    {
        if (!SnapLayout.IsSupported())
            return;

        _snapLayout = SnapLayout.Register(ParentWindow, maximizeButton, restoreButton);

        _snapLayout.HoverColorLight = new SolidColorBrush(Color.FromArgb(
            (byte)0x1A,
            (byte)0x00,
            (byte)0x00,
            (byte)0x00)
        );
        _snapLayout.HoverColorDark = new SolidColorBrush(Color.FromArgb(
            (byte)0x17,
            (byte)0xFF,
            (byte)0xFF,
            (byte)0xFF)
        );

        _snapLayout.Theme = Theme;
    }

    private void OnMainGridMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed || ParentWindow == null)
            return;

        Interop.User32.GetCursorPos(out var currentMousePos);

        if (currentMousePos.x == _doubleClickPoint.x && currentMousePos.y == _doubleClickPoint.y)
            return;

        if (IsMaximized)
        {
            var screenPoint = PointToScreen(e.MouseDevice.GetPosition(this));
            var systemDpi = DpiHelper.GetSystemDpi();

            screenPoint.X /= systemDpi.DpiScaleX;
            screenPoint.Y /= systemDpi.DpiScaleY;

            ParentWindow.Left = screenPoint.X - (ParentWindow.RestoreBounds.Width * 0.5);
            ParentWindow.Top = screenPoint.Y;

            var style = ParentWindow.WindowStyle;
            ParentWindow.WindowStyle = WindowStyle.None;
            ParentWindow.WindowState = WindowState.Normal;
            ParentWindow.WindowStyle = style;
        }

        if (e.LeftButton == MouseButtonState.Pressed)
            ParentWindow.DragMove();
    }

    private void OnParentWindowStateChanged(object sender, EventArgs e)
    {
        if (ParentWindow == null)
            return;

        if (IsMaximized != (ParentWindow.WindowState == WindowState.Maximized))
            IsMaximized = ParentWindow.WindowState == WindowState.Maximized;
    }

    private void OnMainGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount != 2)
            return;

        Interop.User32.GetCursorPos(out _doubleClickPoint);

        MaximizeWindow();
    }

    private void OnTemplateButtonClick(TitleBar sender, object parameter)
    {
        string command = parameter as string;

        switch (command)
        {
            case "maximize":
                RaiseEvent(new RoutedEventArgs(MaximizeClickedEvent, this));
                MaximizeWindow();
                break;

            case "restore":
                RaiseEvent(new RoutedEventArgs(MaximizeClickedEvent, this));
                RestoreWindow();
                break;

            case "close":
                RaiseEvent(new RoutedEventArgs(CloseClickedEvent, this));
                CloseWindow();
                break;

            case "minimize":
                RaiseEvent(new RoutedEventArgs(MinimizeClickedEvent, this));
                MinimizeWindow();
                break;

            case "help":
                RaiseEvent(new RoutedEventArgs(HelpClickedEvent, this));
                break;
        }
    }
}
