using System.Collections.Generic;

namespace Wpf.Ui.Tray;

internal static class TrayData
{
    public static List<INotifyIcon> NotifyIcons { get; set; } = new();
}

