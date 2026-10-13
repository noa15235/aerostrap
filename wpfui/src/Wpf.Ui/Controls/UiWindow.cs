using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Shell;
using Wpf.Ui.Appearance;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(UiWindow), "UiWindow.bmp")]
public class UiWindow : System.Windows.Window
{
    #region Private properties

    private WindowInteropHelper _interopHelper = null;

    private HwndSource _hwndSource = null;

    #endregion Private properties

    #region Protected properties

    protected WindowInteropHelper InteropHelper
    {
        get => _interopHelper ??= new WindowInteropHelper(this);
    }

    protected HwndSource HwndSource
    {
        get => _hwndSource ??= HwndSource.FromHwnd(InteropHelper.Handle);
    }

    #endregion Protected properties

    #region Public properties

    public static readonly DependencyProperty ExtendsContentIntoTitleBarProperty = DependencyProperty.Register(
        nameof(ExtendsContentIntoTitleBar),
        typeof(bool), typeof(UiWindow), new PropertyMetadata(false, OnExtendsContentIntoTitleBarChanged));

    public static readonly DependencyProperty WindowCornerPreferenceProperty = DependencyProperty.Register(
        nameof(WindowCornerPreference),
        typeof(WindowCornerPreference), typeof(UiWindow),
        new PropertyMetadata(WindowCornerPreference.Round, OnCornerPreferenceChanged));

    public static readonly DependencyProperty WindowBackdropTypeProperty = DependencyProperty.Register(
        nameof(WindowBackdropType),
        typeof(BackgroundType), typeof(UiWindow), new PropertyMetadata(BackgroundType.None, OnBackdropTypeChanged));

    public static readonly DependencyProperty DefaultBorderEnabledProperty = DependencyProperty.Register(
        nameof(DefaultBorderEnabled),
        typeof(bool), typeof(UiWindow), new PropertyMetadata(true));

    public static readonly DependencyProperty DefaultBorderThemeOverwriteProperty = DependencyProperty.Register(
        nameof(DefaultBorderThemeOverwrite),
        typeof(ThemeType), typeof(UiWindow), new PropertyMetadata(ThemeType.Unknown));

    public bool ExtendsContentIntoTitleBar
    {
        get => (bool)GetValue(ExtendsContentIntoTitleBarProperty);
        set => SetValue(ExtendsContentIntoTitleBarProperty, value);
    }

    public WindowCornerPreference WindowCornerPreference
    {
        get => (WindowCornerPreference)GetValue(WindowCornerPreferenceProperty);
        set => SetValue(WindowCornerPreferenceProperty, value);
    }

    public BackgroundType WindowBackdropType
    {
        get => (BackgroundType)GetValue(WindowBackdropTypeProperty);
        set => SetValue(WindowBackdropTypeProperty, value);
    }

    public bool DefaultBorderEnabled
    {
        get => (bool)GetValue(DefaultBorderEnabledProperty);
        set => SetValue(DefaultBorderEnabledProperty, value);
    }

    public ThemeType DefaultBorderThemeOverwrite
    {
        get => (ThemeType)GetValue(DefaultBorderThemeOverwriteProperty);
        set => SetValue(DefaultBorderThemeOverwriteProperty, value);
    }

    #endregion Public properties

    #region Constructors

    public UiWindow()
    {
        SetResourceReference(StyleProperty, typeof(UiWindow));
    }

    static UiWindow()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(typeof(UiWindow)));
        HeightProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(600d));
        WidthProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(1100d));
        MinHeightProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(320d));
        MinWidthProperty.OverrideMetadata(typeof(UiWindow), new FrameworkPropertyMetadata(460d));
    }

    #endregion Constructors

    #region Protected methods

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        ApplyWindowCornerPreferenceInternal(WindowCornerPreference);
        ApplyWindowBackdropInternal(WindowBackdropType);
        ExtendsContentIntoTitleBarInternal(ExtendsContentIntoTitleBar);
    }

    protected virtual void OnExtendsContentIntoTitleBarChanged(bool oldValue, bool newValue)
    {
        if (oldValue == newValue)
            return;

        ExtendsContentIntoTitleBarInternal(newValue);
    }

    private static void OnExtendsContentIntoTitleBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not UiWindow window)
            return;

        window.OnExtendsContentIntoTitleBarChanged((bool)e.OldValue, (bool)e.NewValue);
    }

    protected virtual void OnCornerPreferenceChanged(WindowCornerPreference oldValue, WindowCornerPreference newValue)
    {
        if (oldValue == newValue)
            return;

        ApplyWindowCornerPreferenceInternal(newValue);
    }

    private static void OnCornerPreferenceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not UiWindow window)
            return;

        window.OnCornerPreferenceChanged((WindowCornerPreference)e.OldValue, (WindowCornerPreference)e.NewValue);
    }

    protected virtual void OnBackdropTypeChanged(BackgroundType oldValue, BackgroundType newValue)
    {
        if (oldValue == newValue)
            return;

        ApplyWindowBackdropInternal(newValue);
    }

    private static void OnBackdropTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not UiWindow window)
            return;

        window.OnBackdropTypeChanged((BackgroundType)e.OldValue, (BackgroundType)e.NewValue);
    }

    #endregion Protected methods

    #region Private methods

    private void ApplyWindowCornerPreferenceInternal(WindowCornerPreference cornerPreference)
    {
        if (InteropHelper.Handle == IntPtr.Zero)
            return;

        UnsafeNativeMethods.ApplyWindowCornerPreference(InteropHelper.Handle, cornerPreference);
    }

    private void ApplyWindowBackdropInternal(BackgroundType backdropType)
    {
        if (backdropType == BackgroundType.Unknown || backdropType == BackgroundType.None)
        {
            Appearance.Background.Remove(this);

            return;
        }

        if (!ExtendsContentIntoTitleBar)
            throw new InvalidOperationException($"Cannot apply backdrop effect if {nameof(ExtendsContentIntoTitleBar)} is false.");

        if (backdropType == BackgroundType.Aero && !AllowsTransparency)
            throw new InvalidOperationException("Aero effect requires AllowsTransparency to be set to True.");

        if (backdropType == BackgroundType.Acrylic && !Win32.Utilities.IsOSWindows11Insider1OrNewer &&
            !AllowsTransparency)
            throw new InvalidOperationException("In the Windows system below 22523 build, the Acrylic effect cannot be applied if the Window does not have AllowsTransparency set to True.");

        Appearance.Background.Apply(this, WindowBackdropType);
    }

    private void ExtendsContentIntoTitleBarInternal(bool extendContent)
    {
        if (!extendContent)
        {
            WindowChrome.SetWindowChrome(this, null);

            return;
        }

        UnsafeNativeMethods.RemoveWindowTitlebar(this);

        bool win11beta = Environment.OSVersion.Version.Build > 21000 && Environment.OSVersion.Version.Build < 22000;

        WindowChrome.SetWindowChrome(this,
            new WindowChrome
            {
                CaptionHeight = 1,
                CornerRadius = new CornerRadius(4),
                GlassFrameThickness = new Thickness(win11beta ? 0 : -1),
                ResizeBorderThickness = this.ResizeMode == ResizeMode.NoResize ? new Thickness(0) : new Thickness(4),
                UseAeroCaptionButtons = false
            });
    }

    #endregion Private methods
}
