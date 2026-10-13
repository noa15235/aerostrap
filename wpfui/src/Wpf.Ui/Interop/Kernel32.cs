using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop;

internal class Kernel32
{
    [DllImport(Libraries.Kernel32)]
    public static extern int GetLastError();

    [DllImport(Libraries.Kernel32, ExactSpelling = true, CharSet = CharSet.Auto)]
    public static extern void SetLastError([In] int dwErrorCode);

    [DllImport(Libraries.Kernel32, ExactSpelling = true, CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsDebuggerPresent();
}
