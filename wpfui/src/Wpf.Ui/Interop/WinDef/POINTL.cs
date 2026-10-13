using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop.WinDef;

[StructLayout(LayoutKind.Sequential)]
public struct POINTL
{
    public long x;

    public long y;
}
