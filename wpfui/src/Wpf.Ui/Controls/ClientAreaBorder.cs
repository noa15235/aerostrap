#nullable enable

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shell;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Dpi;
using Size = System.Windows.Size;

namespace Wpf.Ui.Controls;

public class ClientAreaBorder : System.Windows.Controls.Border, IThemeControl
{
    private const int SM_CXFRAME = 32;

    private const int SM_CYFRAME = 33;

    private const int SM_CXPADDEDBORDER = 92;

    private Window? _oldWindow;

    private static Thickness? _paddedBorderThickness;

    private static Thickness? _resizeFrameBorderThickness;

    private static Thickness? _windowChromeNonClientFrameThickness;

    public ThemeType Theme { get; set; } = ThemeType.Unknown;

    public Thickness PaddedBorderThickness
    {
        get
        {
            if (_paddedBorderThickness is not null)
                return _paddedBorderThickness.Value;

            var paddedBorder = Interop.User32.GetSystemMetrics(
                Interop.User32.SM.CXPADDEDBORDER);

            var (factorX, factorY) = GetDpi();
            var frameSize = new Size(paddedBorder, paddedBorder);
            var frameSizeInDips = new Size(frameSize.Width / factorX, frameSize.Height / factorY);

            _paddedBorderThickness = new Thickness(frameSizeInDips.Width, frameSizeInDips.Height,
                frameSizeInDips.Width, frameSizeInDips.Height);

            return _paddedBorderThickness.Value;
        }
    }

    public Thickness ResizeFrameBorderThickness => _resizeFrameBorderThickness ??= new Thickness(
        SystemParameters.ResizeFrameVerticalBorderWidth,
        SystemParameters.ResizeFrameHorizontalBorderHeight,
        SystemParameters.ResizeFrameVerticalBorderWidth,
        SystemParameters.ResizeFrameHorizontalBorderHeight);

    public Thickness WindowChromeNonClientFrameThickness => _windowChromeNonClientFrameThickness ??= new Thickness(
        ResizeFrameBorderThickness.Left + PaddedBorderThickness.Left,
        ResizeFrameBorderThickness.Top + PaddedBorderThickness.Top,
        ResizeFrameBorderThickness.Right + PaddedBorderThickness.Right,
        ResizeFrameBorderThickness.Bottom + PaddedBorderThickness.Bottom);

    public ClientAreaBorder()
    {
        Theme = Appearance.Theme.GetAppTheme();
        Appearance.Theme.Changed += OnThemeChanged;
    }

    private void OnThemeChanged(ThemeType currentTheme, Color systemAccent)
    {
        Theme = currentTheme;

        ApplyDefaultWindowBorder();
    }

    protected override void OnVisualParentChanged(DependencyObject oldParent)
    {
        base.OnVisualParentChanged(oldParent);

        if (_oldWindow is { } oldWindow)
        {
            oldWindow.StateChanged -= OnWindowStateChanged;
        }

        var newWindow = (Window?)Window.GetWindow(this);

        if (newWindow is not null)
        {
            newWindow.StateChanged -= OnWindowStateChanged;
            newWindow.StateChanged += OnWindowStateChanged;
        }

        _oldWindow = newWindow;

        ApplyDefaultWindowBorder();
    }

    private void OnWindowStateChanged(object? sender, EventArgs e)
    {
        if (sender is not Window window)
            return;

        Padding = window.WindowState switch
        {
            WindowState.Maximized => WindowChromeNonClientFrameThickness,
            _ => default,
        };
    }

    private void ApplyDefaultWindowBorder()
    {
        if (Win32.Utilities.IsOSWindows11OrNewer || _oldWindow == null)
        {
            ResetDefaultWindowBorder(_oldWindow);
            return;
        }

        ThemeType theme = Theme;

        if (_oldWindow is UiWindow uiWindow)
        {
            if (!uiWindow.DefaultBorderEnabled)
            {
                ResetDefaultWindowBorder(uiWindow);
                return;
            }

            if (uiWindow.DefaultBorderThemeOverwrite != ThemeType.Unknown)
                theme = uiWindow.DefaultBorderThemeOverwrite;
        }

        Brush brush = new SolidColorBrush(theme == ThemeType.Light ? Color.FromArgb(255, 200, 200, 200) : Color.FromArgb(255, 58, 58, 58));

        BorderThickness = new Thickness(1);
        BorderBrush = brush;
    }

    private void ResetDefaultWindowBorder(Window? window)
    {
        if (window == null)
            return;

        BorderBrush = window.BorderBrush;
        BorderThickness = window.BorderThickness;
    }

    private (double factorX, double factorY) GetDpi()
    {
        if (PresentationSource.FromVisual(this) is { } source)
            return (source.CompositionTarget.TransformToDevice.M11,
                source.CompositionTarget.TransformToDevice.M22);

        var systemDPi = DpiHelper.GetSystemDpi();

        return (systemDPi.DpiScaleX, systemDPi.DpiScaleY);
    }
}
