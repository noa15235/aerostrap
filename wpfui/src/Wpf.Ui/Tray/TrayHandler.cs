using System;
using System.Windows.Interop;

namespace Wpf.Ui.Tray;

internal class TrayHandler : HwndSource
{
    public int ElementId { get; internal set; }

    public TrayHandler(string name, IntPtr parent)
        : base(0x0, 0x4000000, 0x80000 | 0x20 | 0x00000008 | 0x08000000, 0, 0, 0, 0, name, parent)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | New {typeof(TrayHandler)} registered with handle: #{Handle}, and parent: #{parent}", "Wpf.Ui.TrayHandler");
#endif
    }
}
