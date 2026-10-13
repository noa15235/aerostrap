using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;

namespace Wpf.Ui.Interop;

internal static class User32
{
    [Flags]
    public enum SWP
    {
        ASYNCWINDOWPOS = 0x4000,
        DEFERERASE = 0x2000,
        DRAWFRAME = 0x0020,
        FRAMECHANGED = 0x0020,
        HIDEWINDOW = 0x0080,
        NOACTIVATE = 0x0010,
        NOCOPYBITS = 0x0100,
        NOMOVE = 0x0002,
        NOOWNERZORDER = 0x0200,
        NOREDRAW = 0x0008,
        NOREPOSITION = 0x0200,
        NOSENDCHANGING = 0x0400,
        NOSIZE = 0x0001,
        NOZORDER = 0x0004,
        SHOWWINDOW = 0x0040,
    }

    [Flags]
    public enum MF : uint
    {
        DOES_NOT_EXIST = unchecked((uint)-1),
        ENABLED = 0,
        BYCOMMAND = 0,
        GRAYED = 1,
        DISABLED = 2,
    }

    public enum SC
    {
        SIZE = 0xF000,
        MOVE = 0xF010,
        MINIMIZE = 0xF020,
        MAXIMIZE = 0xF030,
        NEXTWINDOW = 0xF040,
        PREVWINDOW = 0xF050,
        CLOSE = 0xF060,
        VSCROLL = 0xF070,
        HSCROLL = 0xF080,
        MOUSEMENU = 0xF090,
        KEYMENU = 0xF100,
        ARRANGE = 0xF110,
        RESTORE = 0xF120,
        TASKLIST = 0xF130,
        SCREENSAVE = 0xF140,
        HOTKEY = 0xF150,
        DEFAULT = 0xF160,
        MONITORPOWER = 0xF170,
        CONTEXTHELP = 0xF180,
        SEPARATOR = 0xF00F,
        F_ISSECURE = 0x00000001,
        ICON = MINIMIZE,
        ZOOM = MAXIMIZE,
    }

    public enum WM_NCHITTEST
    {
        HTERROR = unchecked(-2),

        HTTRANSPARENT = unchecked(-1),

        HTNOWHERE = 0,

        HTCLIENT = 1,

        HTCAPTION = 2,

        HTSYSMENU = 3,

        HTGROWBOX = 4,
        HTSIZE = HTGROWBOX,

        HTMENU = 5,

        HTHSCROLL = 6,

        HTVSCROLL = 7,

        HTMINBUTTON = 8,

        HTMAXBUTTON = 9,

        HTLEFT = 10,

        HTRIGHT = 11,

        HTTOP = 12,

        HTTOPLEFT = 13,
        HTTOPRIGHT = 14,
        HTBOTTOM = 15,
        HTBOTTOMLEFT = 16,
        HTBOTTOMRIGHT = 17,
        HTBORDER = 18,
        HTREDUCE = HTMINBUTTON,
        HTZOOM = HTMAXBUTTON,
        HTSIZEFIRST = HTLEFT,
        HTSIZELAST = HTBOTTOMRIGHT,
        HTOBJECT = 19,
        HTCLOSE = 20,
        HTHELP = 21
    }

    [Flags]
    public enum GWL
    {
        GWL_EXSTYLE = -20,

        GWLP_HINSTANCE = -6,

        GWLP_HWNDPARENT = -8,

        GWL_ID = -12,

        GWL_STYLE = -16,

        GWL_USERDATA = -21,

        GWL_WNDPROC = -4,

        DWLP_USER = 0x8,

        DWLP_MSGRESULT = 0x0,

        DWLP_DLGPROC = 0x4
    }

    public enum WCA
    {
        WCA_UNDEFINED = 0,
        WCA_NCRENDERING_ENABLED = 1,
        WCA_NCRENDERING_POLICY = 2,
        WCA_TRANSITIONS_FORCEDISABLED = 3,
        WCA_ALLOW_NCPAINT = 4,
        WCA_CAPTION_BUTTON_BOUNDS = 5,
        WCA_NONCLIENT_RTL_LAYOUT = 6,
        WCA_FORCE_ICONIC_REPRESENTATION = 7,
        WCA_EXTENDED_FRAME_BOUNDS = 8,
        WCA_HAS_ICONIC_BITMAP = 9,
        WCA_THEME_ATTRIBUTES = 10,
        WCA_NCRENDERING_EXILED = 11,
        WCA_NCADORNMENTINFO = 12,
        WCA_EXCLUDED_FROM_LIVEPREVIEW = 13,
        WCA_VIDEO_OVERLAY_ACTIVE = 14,
        WCA_FORCE_ACTIVEWINDOW_APPEARANCE = 15,
        WCA_DISALLOW_PEEK = 16,
        WCA_CLOAK = 17,
        WCA_CLOAKED = 18,
        WCA_ACCENT_POLICY = 19,
        WCA_FREEZE_REPRESENTATION = 20,
        WCA_EVER_UNCLOAKED = 21,
        WCA_VISUAL_OWNER = 22,
        WCA_HOLOGRAPHIC = 23,
        WCA_EXCLUDED_FROM_DDA = 24,
        WCA_PASSIVEUPDATEMODE = 25,
        WCA_USEDARKMODECOLORS = 26,
        WCA_CORNER_STYLE = 27,
        WCA_PART_COLOR = 28,
        WCA_DISABLE_MOVESIZE_FEEDBACK = 29,
        WCA_LAST = 30
    }

    [Flags]
    public enum ACCENT_FLAGS
    {
        DrawLeftBorder = 0x20,
        DrawTopBorder = 0x40,
        DrawRightBorder = 0x80,
        DrawBottomBorder = 0x100,
        DrawAllBorders = DrawLeftBorder | DrawTopBorder | DrawRightBorder | DrawBottomBorder
    }

    public enum ACCENT_STATE
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND = 3,
        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
        ACCENT_INVALID_STATE = 5
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ACCENT_POLICY
    {
        public ACCENT_STATE nAccentState;
        public uint nFlags;
        public uint nColor;
        public uint nAnimationId;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINCOMPATTRDATA
    {
        public WCA Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }

    [Flags]
    public enum CS : uint
    {
        VREDRAW = 0x0001,
        HREDRAW = 0x0002,
        DBLCLKS = 0x0008,
        OWNDC = 0x0020,
        CLASSDC = 0x0040,
        PARENTDC = 0x0080,
        NOCLOSE = 0x0200,
        SAVEBITS = 0x0800,
        BYTEALIGNCLIENT = 0x1000,
        BYTEALIGNWINDOW = 0x2000,
        GLOBALCLASS = 0x4000,
        IME = 0x00010000,
        DROPSHADOW = 0x00020000
    }

    public enum MSGFLT
    {

        RESET = 0,

        ALLOW = 1,

        DISALLOW = 2,

    }

    public enum MSGFLTINFO
    {
        NONE = 0,
        ALREADYALLOWED_FORWND = 1,
        ALREADYDISALLOWED_FORWND = 2,
        ALLOWED_HIGHER = 3,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CHANGEFILTERSTRUCT
    {
        public uint cbSize;
        public MSGFLTINFO ExtStatus;
    }

    public enum WM
    {
        NULL = 0x0000,
        CREATE = 0x0001,
        DESTROY = 0x0002,
        MOVE = 0x0003,
        SIZE = 0x0005,
        ACTIVATE = 0x0006,
        SETFOCUS = 0x0007,
        KILLFOCUS = 0x0008,
        ENABLE = 0x000A,
        SETREDRAW = 0x000B,
        SETTEXT = 0x000C,
        GETTEXT = 0x000D,
        GETTEXTLENGTH = 0x000E,
        PAINT = 0x000F,
        CLOSE = 0x0010,
        QUERYENDSESSION = 0x0011,
        QUIT = 0x0012,
        QUERYOPEN = 0x0013,
        ERASEBKGND = 0x0014,
        SYSCOLORCHANGE = 0x0015,
        SHOWWINDOW = 0x0018,
        CTLCOLOR = 0x0019,
        WININICHANGE = 0x001A,
        SETTINGCHANGE = 0x001A,
        ACTIVATEAPP = 0x001C,
        SETCURSOR = 0x0020,
        MOUSEACTIVATE = 0x0021,
        CHILDACTIVATE = 0x0022,
        QUEUESYNC = 0x0023,
        GETMINMAXINFO = 0x0024,

        WINDOWPOSCHANGING = 0x0046,
        WINDOWPOSCHANGED = 0x0047,

        CONTEXTMENU = 0x007B,
        STYLECHANGING = 0x007C,
        STYLECHANGED = 0x007D,
        DISPLAYCHANGE = 0x007E,
        GETICON = 0x007F,
        SETICON = 0x0080,
        NCCREATE = 0x0081,
        NCDESTROY = 0x0082,
        NCCALCSIZE = 0x0083,
        NCHITTEST = 0x0084,
        NCPAINT = 0x0085,
        NCACTIVATE = 0x0086,
        GETDLGCODE = 0x0087,
        SYNCPAINT = 0x0088,
        NCMOUSEMOVE = 0x00A0,
        NCLBUTTONDOWN = 0x00A1,
        NCLBUTTONUP = 0x00A2,
        NCLBUTTONDBLCLK = 0x00A3,
        NCRBUTTONDOWN = 0x00A4,
        NCRBUTTONUP = 0x00A5,
        NCRBUTTONDBLCLK = 0x00A6,
        NCMBUTTONDOWN = 0x00A7,
        NCMBUTTONUP = 0x00A8,
        NCMBUTTONDBLCLK = 0x00A9,

        SYSKEYDOWN = 0x0104,
        SYSKEYUP = 0x0105,
        SYSCHAR = 0x0106,
        SYSDEADCHAR = 0x0107,
        COMMAND = 0x0111,
        SYSCOMMAND = 0x0112,

        MOUSEMOVE = 0x0200,
        LBUTTONDOWN = 0x0201,
        LBUTTONUP = 0x0202,
        LBUTTONDBLCLK = 0x0203,
        RBUTTONDOWN = 0x0204,
        RBUTTONUP = 0x0205,
        RBUTTONDBLCLK = 0x0206,
        MBUTTONDOWN = 0x0207,
        MBUTTONUP = 0x0208,
        MBUTTONDBLCLK = 0x0209,
        MOUSEWHEEL = 0x020A,
        XBUTTONDOWN = 0x020B,
        XBUTTONUP = 0x020C,
        XBUTTONDBLCLK = 0x020D,
        MOUSEHWHEEL = 0x020E,
        PARENTNOTIFY = 0x0210,

        CAPTURECHANGED = 0x0215,
        POWERBROADCAST = 0x0218,
        DEVICECHANGE = 0x0219,

        ENTERSIZEMOVE = 0x0231,
        EXITSIZEMOVE = 0x0232,

        IME_SETCONTEXT = 0x0281,
        IME_NOTIFY = 0x0282,
        IME_CONTROL = 0x0283,
        IME_COMPOSITIONFULL = 0x0284,
        IME_SELECT = 0x0285,
        IME_CHAR = 0x0286,
        IME_REQUEST = 0x0288,
        IME_KEYDOWN = 0x0290,
        IME_KEYUP = 0x0291,

        NCMOUSELEAVE = 0x02A2,

        TABLET_DEFBASE = 0x02C0,

        TABLET_ADDED = TABLET_DEFBASE + 8,
        TABLET_DELETED = TABLET_DEFBASE + 9,
        TABLET_FLICK = TABLET_DEFBASE + 11,
        TABLET_QUERYSYSTEMGESTURESTATUS = TABLET_DEFBASE + 12,

        CUT = 0x0300,
        COPY = 0x0301,
        PASTE = 0x0302,
        CLEAR = 0x0303,
        UNDO = 0x0304,
        RENDERFORMAT = 0x0305,
        RENDERALLFORMATS = 0x0306,
        DESTROYCLIPBOARD = 0x0307,
        DRAWCLIPBOARD = 0x0308,
        PAINTCLIPBOARD = 0x0309,
        VSCROLLCLIPBOARD = 0x030A,
        SIZECLIPBOARD = 0x030B,
        ASKCBFORMATNAME = 0x030C,
        CHANGECBCHAIN = 0x030D,
        HSCROLLCLIPBOARD = 0x030E,
        QUERYNEWPALETTE = 0x030F,
        PALETTEISCHANGING = 0x0310,
        PALETTECHANGED = 0x0311,
        HOTKEY = 0x0312,
        PRINT = 0x0317,
        PRINTCLIENT = 0x0318,
        APPCOMMAND = 0x0319,
        THEMECHANGED = 0x031A,

        DWMCOMPOSITIONCHANGED = 0x031E,
        DWMNCRENDERINGCHANGED = 0x031F,
        DWMCOLORIZATIONCOLORCHANGED = 0x0320,
        DWMWINDOWMAXIMIZEDCHANGE = 0x0321,

        GETTITLEBARINFOEX = 0x033F,

        #region Windows 7

        DWMSENDICONICTHUMBNAIL = 0x0323,
        DWMSENDICONICLIVEPREVIEWBITMAP = 0x0326,

        #endregion

        USER = 0x0400,

        TRAYMOUSEMESSAGE = 0x800,
        APP = 0x8000,
    }

    [Flags]
    public enum WS : long
    {
        OVERLAPPED = 0x00000000,
        POPUP = 0x80000000,
        CHILD = 0x40000000,
        MINIMIZE = 0x20000000,
        VISIBLE = 0x10000000,
        DISABLED = 0x08000000,
        CLIPSIBLINGS = 0x04000000,
        CLIPCHILDREN = 0x02000000,
        MAXIMIZE = 0x01000000,
        BORDER = 0x00800000,
        DLGFRAME = 0x00400000,
        VSCROLL = 0x00200000,
        HSCROLL = 0x00100000,
        SYSMENU = 0x00080000,
        THICKFRAME = 0x00040000,
        GROUP = 0x00020000,
        TABSTOP = 0x00010000,

        MINIMIZEBOX = 0x00020000,
        MAXIMIZEBOX = 0x00010000,

        CAPTION = BORDER | DLGFRAME,
        TILED = OVERLAPPED,
        ICONIC = MINIMIZE,
        SIZEBOX = THICKFRAME,
        TILEDWINDOW = OVERLAPPEDWINDOW,

        OVERLAPPEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX,
        POPUPWINDOW = POPUP | BORDER | SYSMENU,
        CHILDWINDOW = CHILD,
    }

    [Flags]
    public enum WS_EX : long
    {
        NONE = 0,
        DLGMODALFRAME = 0x00000001,
        NOPARENTNOTIFY = 0x00000004,
        TOPMOST = 0x00000008,
        ACCEPTFILES = 0x00000010,
        TRANSPARENT = 0x00000020,
        MDICHILD = 0x00000040,
        TOOLWINDOW = 0x00000080,
        WINDOWEDGE = 0x00000100,
        CLIENTEDGE = 0x00000200,
        CONTEXTHELP = 0x00000400,
        RIGHT = 0x00001000,
        LEFT = 0x00000000,
        RTLREADING = 0x00002000,
        LTRREADING = 0x00000000,
        LEFTSCROLLBAR = 0x00004000,
        RIGHTSCROLLBAR = 0x00000000,
        CONTROLPARENT = 0x00010000,
        STATICEDGE = 0x00020000,
        APPWINDOW = 0x00040000,
        LAYERED = 0x00080000,
        NOINHERITLAYOUT = 0x00100000,
        LAYOUTRTL = 0x00400000,
        COMPOSITED = 0x02000000,
        NOACTIVATE = 0x08000000,
        OVERLAPPEDWINDOW = (WINDOWEDGE | CLIENTEDGE),
        PALETTEWINDOW = (WINDOWEDGE | TOOLWINDOW | TOPMOST),
    }

    public enum SM
    {
        CXSCREEN = 0,
        CYSCREEN = 1,
        CXVSCROLL = 2,
        CYHSCROLL = 3,
        CYCAPTION = 4,
        CXBORDER = 5,
        CYBORDER = 6,
        CXFIXEDFRAME = 7,
        CYFIXEDFRAME = 8,
        CYVTHUMB = 9,
        CXHTHUMB = 10,
        CXICON = 11,
        CYICON = 12,
        CXCURSOR = 13,
        CYCURSOR = 14,
        CYMENU = 15,
        CXFULLSCREEN = 16,
        CYFULLSCREEN = 17,
        CYKANJIWINDOW = 18,
        MOUSEPRESENT = 19,
        CYVSCROLL = 20,
        CXHSCROLL = 21,
        DEBUG = 22,
        SWAPBUTTON = 23,
        CXMIN = 28,
        CYMIN = 29,
        CXSIZE = 30,
        CYSIZE = 31,
        CXFRAME = 32,
        CXSIZEFRAME = CXFRAME,
        CYFRAME = 33,
        CYSIZEFRAME = CYFRAME,
        CXMINTRACK = 34,
        CYMINTRACK = 35,
        CXDOUBLECLK = 36,
        CYDOUBLECLK = 37,
        CXICONSPACING = 38,
        CYICONSPACING = 39,
        MENUDROPALIGNMENT = 40,
        PENWINDOWS = 41,
        DBCSENABLED = 42,
        CMOUSEBUTTONS = 43,
        SECURE = 44,
        CXEDGE = 45,
        CYEDGE = 46,
        CXMINSPACING = 47,
        CYMINSPACING = 48,
        CXSMICON = 49,
        CYSMICON = 50,
        CYSMCAPTION = 51,
        CXSMSIZE = 52,
        CYSMSIZE = 53,
        CXMENUSIZE = 54,
        CYMENUSIZE = 55,
        ARRANGE = 56,
        CXMINIMIZED = 57,
        CYMINIMIZED = 58,
        CXMAXTRACK = 59,
        CYMAXTRACK = 60,
        CXMAXIMIZED = 61,
        CYMAXIMIZED = 62,
        NETWORK = 63,
        CLEANBOOT = 67,
        CXDRAG = 68,
        CYDRAG = 69,
        SHOWSOUNDS = 70,
        CXMENUCHECK = 71,
        CYMENUCHECK = 72,
        SLOWMACHINE = 73,
        MIDEASTENABLED = 74,
        MOUSEWHEELPRESENT = 75,
        XVIRTUALSCREEN = 76,
        YVIRTUALSCREEN = 77,
        CXVIRTUALSCREEN = 78,
        CYVIRTUALSCREEN = 79,
        CMONITORS = 80,
        SAMEDISPLAYFORMAT = 81,
        IMMENABLED = 82,
        CXFOCUSBORDER = 83,
        CYFOCUSBORDER = 84,
        TABLETPC = 86,
        MEDIACENTER = 87,
        CXPADDEDBORDER = 92,
        REMOTESESSION = 0x1000,
        REMOTECONTROL = 0x2001,
    }

    public enum SW
    {
        HIDE = 0,
        SHOWNORMAL = 1,
        NORMAL = 1,
        SHOWMINIMIZED = 2,
        SHOWMAXIMIZED = 3,
        MAXIMIZE = 3,
        SHOWNOACTIVATE = 4,
        SHOW = 5,
        MINIMIZE = 6,
        SHOWMINNOACTIVE = 7,
        SHOWNA = 8,
        RESTORE = 9,
        SHOWDEFAULT = 10,
        FORCEMINIMIZE = 11,
    }

    [StructLayout(LayoutKind.Sequential)]
    public class WINDOWPLACEMENT
    {
        public int length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
        public int flags;
        public SW showCmd;
        public WinDef.POINT ptMinPosition;
        public WinDef.POINT ptMaxPosition;
        public WinDef.RECT rcNormalPosition;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WNDCLASSEX
    {
        public int cbSize;

        public CS style;

        public WndProc lpfnWndProc;

        public int cbClsExtra;

        public int cbWndExtra;

        public IntPtr hInstance;

        public IntPtr hIcon;

        public IntPtr hCursor;

        public IntPtr hbrBackground;

        [MarshalAs(UnmanagedType.LPWStr)] public string lpszMenuName;

        [MarshalAs(UnmanagedType.LPWStr)] public string lpszClassName;

        public IntPtr hIconSm;
    }

    public delegate IntPtr WndProc(IntPtr hWnd, WM uMsg, IntPtr wParam, IntPtr lParam);

    public delegate IntPtr WndProcHook(IntPtr hWnd, WM uMsg, IntPtr wParam, IntPtr lParam, ref bool handled);

    public delegate IntPtr MessageHandler(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern int ReleaseDC([In] IntPtr hWnd, [In] IntPtr hDC);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AdjustWindowRectEx([In] ref Rect lpRect, [In] WS dwStyle,
        [In][MarshalAs(UnmanagedType.Bool)] bool bMenu, [In] WS_EX dwExStyle);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ChangeWindowMessageFilter([In] WM message, [In] MSGFLT dwFlag);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ChangeWindowMessageFilterEx([In] IntPtr hWnd, [In] WM message, [In] MSGFLT action,
        [In, Out, Optional] ref CHANGEFILTERSTRUCT pChangeFilterStruct);

    [DllImport(Libraries.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PostMessageW([In, Optional] IntPtr hWnd, [In] WM Msg, [In] IntPtr wParam,
        [In] IntPtr lParam);

    [DllImport(Libraries.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PostMessageA([In, Optional] IntPtr hWnd, [In] WM Msg, [In] IntPtr wParam,
        [In] IntPtr lParam);

    [DllImport(Libraries.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PostMessage([In, Optional] IntPtr hWnd, [In] WM Msg, [In] IntPtr wParam,
        [In] IntPtr lParam);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern int SendMessage([In] IntPtr hWnd, [In] WM wMsg, [In] IntPtr wParam, [In] IntPtr lParam);

    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr CreateWindowExW(
        [In] WS_EX dwExStyle,
        [In, Optional] [MarshalAs(UnmanagedType.LPWStr)]
        string lpClassName,
        [In, Optional] [MarshalAs(UnmanagedType.LPWStr)]
        string lpWindowName,
        [In] WS dwStyle,
        [In] int x,
        [In] int y,
        [In] int nWidth,
        [In] int nHeight,
        [In, Optional] IntPtr hWndParent,
        [In, Optional] IntPtr hMenu,
        [In, Optional] IntPtr hInstance,
        [In, Optional] IntPtr lpParam);

    public static IntPtr CreateWindowEx(
        [In] WS_EX dwExStyle,
        [In] string lpClassName,
        [In] string lpWindowName,
        [In] WS dwStyle,
        [In] int x,
        [In] int y,
        [In] int nWidth,
        [In] int nHeight,
        [In, Optional] IntPtr hWndParent,
        [In, Optional] IntPtr hMenu,
        [In, Optional] IntPtr hInstance,
        [In, Optional] IntPtr lpParam)
    {
        IntPtr ret = CreateWindowExW(dwExStyle, lpClassName, lpWindowName, dwStyle, x, y, nWidth, nHeight, hWndParent,
            hMenu, hInstance, lpParam);
        if (IntPtr.Zero == ret)
        {
            throw new Exception("Unable to create a window");
        }

        return ret;
    }

    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern short RegisterClassExW([In] ref WNDCLASSEX lpwcx);

    [DllImport(Libraries.User32, SetLastError = true)]
    public static extern short RegisterClassExA([In] ref WNDCLASSEX lpwcx);

    [DllImport(Libraries.User32, SetLastError = true)]
    public static extern short RegisterClassEx([In] ref WNDCLASSEX lpwcx);

    [DllImport(Libraries.User32, CharSet = CharSet.Unicode)]
    public static extern IntPtr DefWindowProcW([In] IntPtr hWnd, [In] WM Msg, [In] IntPtr wParam, [In] IntPtr lParam);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr DefWindowProcA([In] IntPtr hWnd, [In] WM Msg, [In] IntPtr wParam, [In] IntPtr lParam);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr DefWindowProc([In] IntPtr hWnd, [In] WM Msg, [In] IntPtr wParam, [In] IntPtr lParam);

    [DllImport(Libraries.User32, CharSet = CharSet.Unicode)]
    public static extern long GetWindowLongW([In] IntPtr hWnd, [In] int nIndex);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern long GetWindowLongA([In] IntPtr hWnd, [In] int nIndex);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern long GetWindowLong([In] IntPtr hWnd, [In] int nIndex);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern long GetWindowLong([In] IntPtr hWnd, [In] GWL nIndex);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr GetWindowLongPtrW([In] IntPtr hWnd, [In] int nIndex);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr GetWindowLongPtrA([In] IntPtr hWnd, [In] int nIndex);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr GetWindowLongPtr([In] IntPtr hWnd, [In] int nIndex);

    [DllImport(Libraries.User32, CharSet = CharSet.Unicode)]
    public static extern long SetWindowLongW([In] IntPtr hWnd, [In] int nIndex, [In] long dwNewLong);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern long SetWindowLongA([In] IntPtr hWnd, [In] int nIndex, [In] long dwNewLong);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern long SetWindowLong([In] IntPtr hWnd, [In] int nIndex, [In] long dwNewLong);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern long SetWindowLong([In] IntPtr hWnd, [In] GWL nIndex, [In] long dwNewLong);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern long SetWindowLong([In] IntPtr hWnd, [In] GWL nIndex, [In] WS dwNewLong);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr SetWindowLongPtrW([In] IntPtr hWnd, [In] int nIndex, [In] IntPtr dwNewLong);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr SetWindowLongPtrA([In] IntPtr hWnd, [In] int nIndex, [In] IntPtr dwNewLong);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern IntPtr SetWindowLongPtr([In] IntPtr hWnd, [In] int nIndex, [In] IntPtr dwNewLong);

    [DllImport(Libraries.User32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DestroyIcon([In] IntPtr handle);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindow([In] IntPtr hWnd);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DestroyWindow([In] IntPtr hWnd);

    [DllImport(Libraries.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowPlacement([In] IntPtr hWnd, [In] WINDOWPLACEMENT lpwndpl);

    [DllImport(Libraries.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect([In] IntPtr hWnd, [Out] out Rect lpRect);

    [DllImport(Libraries.User32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible([In] IntPtr hWnd);

    [DllImport(Libraries.User32, ExactSpelling = true)]
    internal static extern bool IsWindowEnabled(IntPtr hWnd);

    [DllImport(Libraries.User32)]
    public static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint dwFlags);

    [DllImport(Libraries.User32)]
    public static extern int GetSystemMetrics([In] SM nIndex);

    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern uint RegisterWindowMessageW([MarshalAs(UnmanagedType.LPWStr)] string lpString);

    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Auto)]
    public static extern uint RegisterWindowMessageA([MarshalAs(UnmanagedType.LPWStr)] string lpString);

    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Auto)]
    public static extern uint RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);

    [DllImport(Libraries.User32, SetLastError = true)]
    public static extern IntPtr SetActiveWindow(IntPtr hWnd);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport(Libraries.User32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos([Out] out WinDef.POINT lpPoint);

    [DllImport(Libraries.User32)]
    public static extern bool UnionRect(out WinDef.RECT rcDst, ref WinDef.RECT rc1, ref WinDef.RECT rc2);

    [DllImport(Libraries.User32, SetLastError = true)]
    public static extern bool IntersectRect(ref WinDef.RECT rcDest, ref WinDef.RECT rc1, ref WinDef.RECT rc2);

    [DllImport(Libraries.User32)]
    public static extern IntPtr GetShellWindow();

    [DllImport(Libraries.User32, CharSet = CharSet.Unicode)]
    public static extern int MapVirtualKey(int nVirtKey, int nMapType);

    [DllImport(Libraries.User32)]
    public static extern int GetSysColor(int nIndex);

    [DllImport(Libraries.User32)]
    public static extern IntPtr GetSystemMenu([In] IntPtr hWnd, [In][MarshalAs(UnmanagedType.Bool)] bool bRevert);

    [DllImport(Libraries.User32, EntryPoint = "EnableMenuItem")]
    private static extern int _EnableMenuItem([In] IntPtr hMenu, [In] SC uIDEnableItem, [In] MF uEnable);

    public static MF EnableMenuItem([In] IntPtr hMenu, [In] SC uIDEnableItem, [In] MF uEnable)
    {
        int iRet = _EnableMenuItem(hMenu, uIDEnableItem, uEnable);
        return (MF)iRet;
    }

    [DllImport(Libraries.User32, EntryPoint = "SetWindowRgn", SetLastError = true)]
    private static extern int _SetWindowRgn([In] IntPtr hWnd, [In] IntPtr hRgn, [In][MarshalAs(UnmanagedType.Bool)] bool bRedraw);

    public static void SetWindowRgn([In] IntPtr hWnd, [In] IntPtr hRgn, [In] bool bRedraw)
    {
        int err = _SetWindowRgn(hWnd, hRgn, bRedraw);

        if (0 == err)
        {
            throw new Win32Exception();
        }
    }

    [DllImport(Libraries.User32, EntryPoint = "SetWindowPos", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool _SetWindowPos([In] IntPtr hWnd, [In, Optional] IntPtr hWndInsertAfter, [In] int x, [In] int y, [In] int cx, [In] int cy, [In] SWP uFlags);

    public static bool SetWindowPos([In] IntPtr hWnd, [In, Optional] IntPtr hWndInsertAfter, [In] int x, [In] int y, [In] int cx, [In] int cy, [In] SWP uFlags)
    {
        if (!_SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags))
        {
            return false;
        }

        return true;
    }

    [DllImport(Libraries.User32)]
    public static extern void SetProcessDPIAware();

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern int SetWindowCompositionAttribute([In] IntPtr hWnd, [In, Out] ref WINCOMPATTRDATA data);

    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
    public static extern int GetWindowCompositionAttribute([In] IntPtr hWnd, [In, Out] ref WINCOMPATTRDATA data);

    [DllImport(Libraries.User32, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
    public static extern uint GetDpiForWindow([In] IntPtr hWnd);

    [DllImport(Libraries.User32, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
    public static extern uint GetDpiForWindow([In] HandleRef hwnd);
}
