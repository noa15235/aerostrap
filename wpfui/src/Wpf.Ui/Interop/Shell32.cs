using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Wpf.Ui.Interop;

internal static class Shell32
{
    public enum DOGIF
    {
        DEFAULT = 0x0000,
        TRAVERSE_LINK = 0x0001,
        NO_HDROP = 0x0002,
        NO_URL = 0x0004,
        ONLY_IF_ONE = 0x0008,
    }

    public enum NIM : uint
    {
        ADD = 0,
        MODIFY = 1,
        DELETE = 2,
        SETFOCUS = 3,
        SETVERSION = 4,
    }

    [Flags]
    public enum NIF : uint
    {
        MESSAGE = 0x0001,
        ICON = 0x0002,
        TIP = 0x0004,
        STATE = 0x0008,
        INFO = 0x0010,
        GUID = 0x0020,

        REALTIME = 0x0040,

        SHOWTIP = 0x0080,

        XP_MASK = MESSAGE | ICON | STATE | INFO | GUID,
        VISTA_MASK = XP_MASK | REALTIME | SHOWTIP,
    }

    [StructLayout(LayoutKind.Sequential)]
    public class NOTIFYICONDATA
    {
        public int cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA));

        public IntPtr hWnd;

        public int uID;

        public NIF uFlags;

        public int uCallbackMessage;

        public IntPtr hIcon;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x80)]
        public string szTip;

        public uint dwState;

        public uint dwStateMask;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)]
        public string szInfo;

        public uint uVersion;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x40)]
        public string szInfoTitle;

        public uint dwInfoFlags;

        public Guid guidItem;

        IntPtr hBalloonIcon;
    }

    [DllImport(Libraries.Shell32, PreserveSig = false)]
    public static extern void SHGetItemFromDataObject(IDataObject pdtobj, DOGIF dwFlags, [In] ref Guid riid,
        [Out, MarshalAs(UnmanagedType.Interface)]
        out object ppv);

    [DllImport(Libraries.Shell32)]
    public static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IBindCtx pbc,
        [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out object ppv);

    [DllImport(Libraries.Shell32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool Shell_NotifyIcon([In] NIM dwMessage, [In] NOTIFYICONDATA lpdata);

    [DllImport(Libraries.Shell32, PreserveSig = false)]
    public static extern void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);

    [DllImport(Libraries.Shell32)]
    public static extern int GetCurrentProcessExplicitAppUserModelID(
        [Out, MarshalAs(UnmanagedType.LPWStr)] out string AppID);
}
