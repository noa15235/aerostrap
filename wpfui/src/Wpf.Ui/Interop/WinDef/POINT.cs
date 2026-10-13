using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop.WinDef;

[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    public int x;

    public int y;
}
