using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Extensions;
using Wpf.Ui.Tray;

namespace Wpf.Ui.Services.Internal;

internal class NotifyIconService : IDisposable, INotifyIcon
{
    protected bool Disposed = false;

    public int Id { get; set; } = -1;

    public bool IsRegistered { get; set; } = false;

    public string TooltipText { get; set; } = String.Empty;

    public ImageSource Icon { get; set; } = null!;

    public HwndSource HookWindow { get; set; } = null!;

    public ContextMenu ContextMenu { get; set; } = null!;

    public bool FocusOnLeftClick { get; set; } = true;

    public bool MenuOnRightClick { get; set; } = true;

    #region Events

    public event NotifyIconEventHandler LeftClick;

    public event NotifyIconEventHandler LeftDoubleClick;

    public event NotifyIconEventHandler RightClick;

    public event NotifyIconEventHandler RightDoubleClick;

    public event NotifyIconEventHandler MiddleClick;

    public event NotifyIconEventHandler MiddleDoubleClick;

    #endregion Events

    public Interop.Shell32.NOTIFYICONDATA ShellIconData { get; set; }

    public NotifyIconService()
    {
        Theme.Changed += OnThemeChanged;
    }

    ~NotifyIconService()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    public virtual bool Register()
    {
        IsRegistered = TrayManager.Register(this);

        return IsRegistered;
    }

    public virtual bool Register(Window parentWindow)
    {
        IsRegistered = TrayManager.Register(this, parentWindow);

        return IsRegistered;
    }

    public virtual bool Unregister()
    {
        return TrayManager.Unregister(this);
    }

    protected virtual void OnThemeChanged(ThemeType currentTheme, Color systemAccent)
    {
        ContextMenu?.UpdateDefaultStyle();
        ContextMenu?.UpdateLayout();
    }

    protected virtual void FocusApp()
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TrayHandler)} invoked {nameof(FocusApp)} method.",
            "Wpf.Ui.NotifyIcon");
#endif
        var mainWindow = Application.Current.MainWindow;

        if (mainWindow == null)
            return;

        if (mainWindow.WindowState == WindowState.Minimized)
            mainWindow.WindowState = WindowState.Normal;

        mainWindow.Show();

        if (mainWindow.Topmost)
        {
            mainWindow.Topmost = false;
            mainWindow.Topmost = true;
        }
        else
        {
            mainWindow.Topmost = true;
            mainWindow.Topmost = false;
        }

        mainWindow.Focus();
    }

    protected virtual void OpenMenu()
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TrayHandler)} invoked {nameof(OpenMenu)} method.",
            "Wpf.Ui.NotifyIcon");
#endif
        if (ContextMenu == null)
            return;

        Interop.User32.SetForegroundWindow(HookWindow.Handle);
        ContextMenuService.SetPlacement(ContextMenu, PlacementMode.MousePoint);

        ContextMenu.ApplyMica();
        ContextMenu.IsOpen = true;
    }

    protected virtual void OnLeftClick()
    {
        LeftClick?.Invoke();
    }

    protected virtual void OnLeftDoubleClick()
    {
        LeftDoubleClick?.Invoke();
    }

    protected virtual void OnRightClick()
    {
        RightClick?.Invoke();
    }

    protected virtual void OnRightDoubleClick()
    {
        RightDoubleClick?.Invoke();
    }

    protected virtual void OnMiddleClick()
    {
        MiddleClick?.Invoke();
    }

    protected virtual void OnMiddleDoubleClick()
    {
        MiddleDoubleClick?.Invoke();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (Disposed)
            return;

        Disposed = true;

        if (!disposing)
            return;

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(NotifyIconService)} disposed.", "Wpf.Ui.NotifyIcon");
#endif

        Unregister();
    }

    public IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        var uMsg = (Interop.User32.WM)msg;

        switch (uMsg)
        {
            case Interop.User32.WM.DESTROY:
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TrayHandler)} received {uMsg} message.",
                    "Wpf.Ui.NotifyIcon");
#endif
                Dispose();

                handled = true;

                return IntPtr.Zero;

            case Interop.User32.WM.NCDESTROY:
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TrayHandler)} received {uMsg} message.",
                    "Wpf.Ui.NotifyIcon");
#endif
                handled = false;

                return IntPtr.Zero;

            case Interop.User32.WM.CLOSE:
#if DEBUG
                System.Diagnostics.Debug.WriteLine($"INFO | {typeof(TrayHandler)} received {uMsg} message.",
                    "Wpf.Ui.NotifyIcon");
#endif
                handled = true;

                return IntPtr.Zero;
        }

        if (uMsg != Interop.User32.WM.TRAYMOUSEMESSAGE)
        {
            handled = false;

            return IntPtr.Zero;
        }

        var lMsg = (Interop.User32.WM)lParam;

        switch (lMsg)
        {
            case Interop.User32.WM.LBUTTONDOWN:
                OnLeftClick();

                if (FocusOnLeftClick)
                    FocusApp();
                break;

            case Interop.User32.WM.LBUTTONDBLCLK:
                OnLeftDoubleClick();
                break;

            case Interop.User32.WM.RBUTTONDOWN:
                OnRightClick();

                if (MenuOnRightClick)
                    OpenMenu();
                break;

            case Interop.User32.WM.RBUTTONDBLCLK:
                OnRightDoubleClick();
                break;

            case Interop.User32.WM.MBUTTONDOWN:
                OnMiddleClick();
                break;

            case Interop.User32.WM.MBUTTONDBLCLK:
                OnMiddleDoubleClick();
                break;
        }

        handled = true;

        return IntPtr.Zero;
    }
}
