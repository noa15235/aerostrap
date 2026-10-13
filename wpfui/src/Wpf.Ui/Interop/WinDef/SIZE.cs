using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop.WinDef;

[StructLayout(LayoutKind.Sequential)]
public struct SIZE
{
    public long cx;

    public long cy;
}
