using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Dpi;

namespace Wpf.Ui.TitleBar;

internal sealed class SnapLayout : IThemeControl
{
    private readonly SnapLayoutButton[] _buttons;

    private ThemeType _currentTheme;

    private SolidColorBrush _currentHoverColor = Brushes.Transparent;

    public ThemeType Theme
    {
        get => _currentTheme;
        set
        {
            _currentHoverColor = value == ThemeType.Light ? HoverColorLight : HoverColorDark;
            _currentTheme = value;
        }
    }

    public SolidColorBrush DefaultButtonBackground { get; set; } = Brushes.Transparent;

    public SolidColorBrush HoverColorLight = Brushes.Transparent;

    public SolidColorBrush HoverColorDark = Brushes.Transparent;

    private SnapLayout(Window window, Wpf.Ui.Controls.Button maximizeButton, Wpf.Ui.Controls.Button restoreButton)
    {
        if (window == null)
            return;

        var windowHandle = new WindowInteropHelper(window).Handle;

        if (windowHandle == IntPtr.Zero)
            return;

        var windowDpi = DpiHelper.GetWindowDpi(windowHandle);

        _buttons = new[]
        {
            new SnapLayoutButton(maximizeButton, TitleBarButton.Maximize, windowDpi.DpiScaleX),
            new SnapLayoutButton(restoreButton, TitleBarButton.Restore, windowDpi.DpiScaleX),
        };

        var windowSource = HwndSource.FromHwnd(windowHandle);

        if (windowSource != null)
            windowSource.AddHook(HwndSourceHook);
    }

    public static bool IsSupported()
    {
        return Win32.Utilities.IsOSWindows11OrNewer;
    }

    public static SnapLayout Register(Window window, Wpf.Ui.Controls.Button maximizeButton, Wpf.Ui.Controls.Button restoreButton)
    {
        return new SnapLayout(window, maximizeButton, restoreButton);
    }

    private IntPtr HwndSourceHook(IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        var mouseNotification = (Interop.User32.WM)uMsg;

        switch (mouseNotification)
        {
            case Interop.User32.WM.MOVE:
                break;

            case Interop.User32.WM.NCMOUSELEAVE:
                _buttons[0].RemoveHover(DefaultButtonBackground);
                _buttons[1].RemoveHover(DefaultButtonBackground);

                break;

            case Interop.User32.WM.NCLBUTTONDOWN:
                if (_buttons[0].IsMouseOver(lParam))
                {
                    _buttons[0].IsClickedDown = true;

                    handled = true;
                }

                if (_buttons[1].IsMouseOver(lParam))
                {
                    _buttons[1].IsClickedDown = true;

                    handled = true;
                }

                break;

            case Interop.User32.WM.NCLBUTTONUP:
                if (_buttons[0].IsClickedDown && _buttons[0].IsMouseOver(lParam))
                {
                    _buttons[0].InvokeClick();

                    handled = true;

                    break;
                }

                if (_buttons[1].IsClickedDown && _buttons[1].IsMouseOver(lParam))
                {
                    _buttons[1].InvokeClick();

                    handled = true;
                }

                break;

            case Interop.User32.WM.NCHITTEST:
                if (_buttons[0].IsMouseOver(lParam))
                {
                    _buttons[0].Hover(_currentHoverColor);

                    handled = true;

                    return new IntPtr((int)Interop.User32.WM_NCHITTEST.HTMAXBUTTON);
                }

                _buttons[0].RemoveHover(DefaultButtonBackground);

                if (_buttons[1].IsMouseOver(lParam))
                {
                    _buttons[1].Hover(_currentHoverColor);

                    handled = true;

                    return new IntPtr((int)Interop.User32.WM_NCHITTEST.HTMAXBUTTON);
                }

                _buttons[1].RemoveHover(DefaultButtonBackground);

                return IntPtr.Zero;
        }

        return IntPtr.Zero;
    }
}
