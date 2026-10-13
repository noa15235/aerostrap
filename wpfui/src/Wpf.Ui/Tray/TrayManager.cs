using System;
using System.Windows;
using System.Windows.Interop;

namespace Wpf.Ui.Tray;

internal static class TrayManager
{
    public static bool Register(INotifyIcon notifyIcon)
    {
        return Register(notifyIcon, GetParentSource());
    }

    public static bool Register(INotifyIcon notifyIcon, Window parentWindow)
    {
        if (parentWindow == null)
            return false;

        return Register(notifyIcon, (HwndSource)PresentationSource.FromVisual(parentWindow));
    }

    public static bool Register(INotifyIcon notifyIcon, HwndSource parentSource)
    {
        if (parentSource == null)
        {
            if (!notifyIcon.IsRegistered)
                return false;

            Unregister(notifyIcon);

            return false;
        }

        if (parentSource.Handle == IntPtr.Zero)
            return false;

        if (notifyIcon.IsRegistered)
            Unregister(notifyIcon);

        notifyIcon.Id = TrayData.NotifyIcons.Count + 1;

        notifyIcon.HookWindow =
            new TrayHandler($"wpfui_th_{parentSource.Handle}_{notifyIcon.Id}", parentSource.Handle) { ElementId = notifyIcon.Id };

        notifyIcon.ShellIconData = new Interop.Shell32.NOTIFYICONDATA
        {
            uID = notifyIcon.Id,
            uFlags = Interop.Shell32.NIF.MESSAGE,
            uCallbackMessage = (int)Interop.User32.WM.TRAYMOUSEMESSAGE,
            hWnd = notifyIcon.HookWindow.Handle,
            dwState = 0x2
        };

        if (!String.IsNullOrEmpty(notifyIcon.TooltipText))
        {
            notifyIcon.ShellIconData.szTip = notifyIcon.TooltipText;
            notifyIcon.ShellIconData.uFlags |= Interop.Shell32.NIF.TIP;
        }

        var hIcon = IntPtr.Zero;

        if (notifyIcon.Icon != null)
            hIcon = Hicon.FromSource(notifyIcon.Icon);

        if (hIcon == IntPtr.Zero)
            hIcon = Hicon.FromApp();

        if (hIcon != IntPtr.Zero)
        {
            notifyIcon.ShellIconData.hIcon = hIcon;
            notifyIcon.ShellIconData.uFlags |= Interop.Shell32.NIF.ICON;
        }

        notifyIcon.HookWindow.AddHook(notifyIcon.WndProc);

        Interop.Shell32.Shell_NotifyIcon(Interop.Shell32.NIM.ADD, notifyIcon.ShellIconData);

        TrayData.NotifyIcons.Add(notifyIcon);

        notifyIcon.IsRegistered = true;

        return true;
    }

    public static bool Unregister(INotifyIcon notifyIcon)
    {
        if (notifyIcon.ShellIconData == null || !notifyIcon.IsRegistered)
            return false;

        Interop.Shell32.Shell_NotifyIcon(Interop.Shell32.NIM.DELETE, notifyIcon.ShellIconData);

        notifyIcon.IsRegistered = false;

        return true;
    }

    private static HwndSource GetParentSource()
    {
        var mainWindow = Application.Current.MainWindow;

        if (mainWindow == null)
            return null;

        return (HwndSource)PresentationSource.FromVisual(mainWindow);
    }
}
