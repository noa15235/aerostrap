using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace Wpf.Ui.Tray;

internal interface INotifyIcon
{
    public Interop.Shell32.NOTIFYICONDATA ShellIconData { get; set; }

    bool IsRegistered { get; set; }

    int Id { get; set; }

    string TooltipText { get; set; }

    ImageSource Icon { get; set; }

    HwndSource HookWindow { get; set; }

    ContextMenu ContextMenu { get; set; }

    bool FocusOnLeftClick { get; set; }

    bool MenuOnRightClick { get; set; }

    public IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled);

    bool Register();

    bool Register(Window parentWindow);

    bool Unregister();
}
