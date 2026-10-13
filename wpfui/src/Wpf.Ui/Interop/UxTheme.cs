using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Wpf.Ui.Interop;

internal static class UxTheme
{
    public struct MARGINS
    {
        public int cxLeftWidth;

        public int cxRightWidth;

        public int cyTopHeight;

        public int cyBottomHeight;
    }

    public enum WINDOWTHEMEATTRIBUTETYPE : uint
    {
        WTA_NONCLIENT = 1,
    }

    [Flags]
    public enum WTNCA : uint
    {
        NODRAWCAPTION = 0x00000001,

        NODRAWICON = 0x00000002,

        NOSYSMENU = 0x00000004,

        NOMIRRORHELP = 0x00000008,

        VALIDBITS = NODRAWCAPTION | NODRAWICON | NOMIRRORHELP | NOSYSMENU,
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct WTA_OPTIONS
    {
        public const uint Size = 8;

        [FieldOffset(0)]
        public WTNCA dwFlags;

        [FieldOffset(4)]
        public WTNCA dwMask;
    }

    [DllImport(Libraries.UxTheme, PreserveSig = false)]
    public static extern void SetWindowThemeAttribute([In] IntPtr hWnd, [In] WINDOWTHEMEATTRIBUTETYPE eAttribute, [In] ref WTA_OPTIONS pvAttribute, [In] uint cbAttribute);

    [DllImport(Libraries.UxTheme)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsThemeActive();

    [DllImport(Libraries.UxTheme, CharSet = CharSet.Unicode)]
    public static extern int GetCurrentThemeName(
        [Out] StringBuilder pszThemeFileName,
        [In] int dwMaxNameChars,
        [Out] StringBuilder pszColorBuff,
        [In] int cchMaxColorChars,
        [Out] StringBuilder pszSizeBuff,
        [In] int cchMaxSizeChars);
}
