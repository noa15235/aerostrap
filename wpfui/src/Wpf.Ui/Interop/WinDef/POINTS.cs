using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop.WinDef;

[StructLayout(LayoutKind.Sequential)]
public struct POINTS
{
    public short x;

    public short y;
}
