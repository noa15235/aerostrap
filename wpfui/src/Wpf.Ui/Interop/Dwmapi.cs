using System;
using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop;

internal static class Dwmapi
{
    public enum DWM_CLOAKED
    {
        DWM_CLOAKED_APP = 0x00000001,
        DWM_CLOAKED_SHELL = 0x00000002,
        DWM_CLOAKED_INHERITED = 0x00000004
    }

    public enum GESTURE_TYPE
    {
        GT_PEN_TAP = 0,
        GT_PEN_DOUBLETAP = 1,
        GT_PEN_RIGHTTAP = 2,
        GT_PEN_PRESSANDHOLD = 3,
        GT_PEN_PRESSANDHOLDABORT = 4,
        GT_TOUCH_TAP = 5,
        GT_TOUCH_DOUBLETAP = 6,
        GT_TOUCH_RIGHTTAP = 7,
        GT_TOUCH_PRESSANDHOLD = 8,
        GT_TOUCH_PRESSANDHOLDABORT = 9,
        GT_TOUCH_PRESSANDTAP = 10,
    }

    public enum DWM_TAB_WINDOW_REQUIREMENTS
    {
        DWMTWR_NONE = 0x0000,

        DWMTWR_IMPLEMENTED_BY_SYSTEM = 0x0001,

        DWMTWR_WINDOW_RELATIONSHIP = 0x0002,

        DWMTWR_WINDOW_STYLES = 0x0004,

        DWMTWR_WINDOW_REGION = 0x0008,

        DWMTWR_WINDOW_DWM_ATTRIBUTES = 0x0010,

        DWMTWR_WINDOW_MARGINS = 0x0020,

        DWMTWR_TABBING_ENABLED = 0x0040,

        DWMTWR_USER_POLICY = 0x0080,

        DWMTWR_GROUP_POLICY = 0x0100,

        DWMTWR_APP_COMPAT = 0x0200
    }

    [Flags]
    public enum DWM_WINDOW_CORNER_PREFERENCE
    {
        DEFAULT = 0,
        DONOTROUND = 1,
        ROUND = 2,
        ROUNDSMALL = 3
    }

    [Flags]
    public enum DWMSBT : uint
    {
        DWMSBT_AUTO = 0,

        DWMSBT_DISABLE = 1,

        DWMSBT_MAINWINDOW = 2,

        DWMSBT_TRANSIENTWINDOW = 3,

        DWMSBT_TABBEDWINDOW = 4
    }

    public enum DWMNCRENDERINGPOLICY
    {
        DWMNCRP_USEWINDOWSTYLE,

        DWMNCRP_DISABLED,

        DWMNCRP_ENABLED,

        DWMNCRP_LAST
    }

    public enum DWMFLIP3DWINDOWPOLICY
    {
        DWMFLIP3D_DEFAULT,

        DWMFLIP3D_EXCLUDEBELOW,

        DWMFLIP3D_EXCLUDEABOVE,

        DWMFLIP3D_LAST
    }

    [Flags]
    public enum DWMWINDOWATTRIBUTE
    {
        DWMWA_NCRENDERING_ENABLED = 1,

        DWMWA_NCRENDERING_POLICY = 2,

        DWMWA_TRANSITIONS_FORCEDISABLED = 3,

        DWMWA_ALLOW_NCPAINT = 4,

        DWMWA_CAPTION_BUTTON_BOUNDS = 5,

        DWMWA_NONCLIENT_RTL_LAYOUT = 6,

        DWMWA_FORCE_ICONIC_REPRESENTATION = 7,

        DWMWA_FLIP3D_POLICY = 8,

        DWMWA_EXTENDED_FRAME_BOUNDS = 9,

        DWMWA_HAS_ICONIC_BITMAP = 10,

        DWMWA_DISALLOW_PEEK = 11,

        DWMWA_EXCLUDED_FROM_PEEK = 12,

        DWMWA_CLOAK = 13,

        DWMWA_CLOAKED = 14,

        DWMWA_FREEZE_REPRESENTATION = 15,

        DWMWA_PASSIVE_UPDATE_MODE = 16,

        DWMWA_USE_HOSTBACKDROPBRUSH = 17,

        DMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19,

        DWMWA_USE_IMMERSIVE_DARK_MODE = 20,

        DWMWA_WINDOW_CORNER_PREFERENCE = 33,

        DWMWA_BORDER_COLOR = 34,

        DWMWA_CAPTION_COLOR = 35,

        DWMWA_TEXT_COLOR = 36,

        DWMWA_VISIBLE_FRAME_BORDER_THICKNESS = 37,

        DWMWA_SYSTEMBACKDROP_TYPE = 38,

        DWMWA_MICA_EFFECT = 1029
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DWMCOLORIZATIONPARAMS
    {
        public uint clrColor;

        public uint clrAfterGlow;

        public uint nIntensity;

        public uint clrAfterGlowBalance;

        public uint clrBlurBalance;

        public uint clrGlassReflectionIntensity;

        public bool fOpaque;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct UNSIGNED_RATIO
    {
        public uint uiNumerator;

        public uint uiDenominator;
    }

    public enum DWM_SHOWCONTACT
    {
        DWMSC_DOWN,
        DWMSC_UP,
        DWMSC_DRAG,
        DWMSC_HOLD,
        DWMSC_PENBARREL,
        DWMSC_NONE,
        DWMSC_ALL
    }

    public enum DWM_SOURCE_FRAME_SAMPLING
    {
        DWM_SOURCE_FRAME_SAMPLING_POINT,

        DWM_SOURCE_FRAME_SAMPLING_COVERAGE,

        DWM_SOURCE_FRAME_SAMPLING_LAST
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DWM_TIMING_INFO
    {
        public int cbSize;
        public UNSIGNED_RATIO rateRefresh;
        public ulong qpcRefreshPeriod;
        public UNSIGNED_RATIO rateCompose;
        public ulong qpcVBlank;
        public ulong cRefresh;
        public uint cDXRefresh;
        public ulong qpcCompose;
        public ulong cFrame;
        public uint cDXPresent;
        public ulong cRefreshFrame;
        public ulong cFrameSubmitted;
        public uint cDXPresentSubmitted;
        public ulong cFrameConfirmed;
        public uint cDXPresentConfirmed;
        public ulong cRefreshConfirmed;
        public uint cDXRefreshConfirmed;
        public ulong cFramesLate;
        public uint cFramesOutstanding;
        public ulong cFrameDisplayed;
        public ulong qpcFrameDisplayed;
        public ulong cRefreshFrameDisplayed;
        public ulong cFrameComplete;
        public ulong qpcFrameComplete;
        public ulong cFramePending;
        public ulong qpcFramePending;
        public ulong cFramesDisplayed;
        public ulong cFramesComplete;
        public ulong cFramesPending;
        public ulong cFramesAvailable;
        public ulong cFramesDropped;
        public ulong cFramesMissed;
        public ulong cRefreshNextDisplayed;
        public ulong cRefreshNextPresented;
        public ulong cRefreshesDisplayed;
        public ulong cRefreshesPresented;
        public ulong cRefreshStarted;
        public ulong cPixelsReceived;
        public ulong cPixelsDrawn;
        public ulong cBuffersEmpty;
    }

    public enum DWM_SIT
    {
        NONE,

        DISPLAYFRAME = 1,
    }

    [DllImport(Libraries.Dwmapi, BestFitMapping = false)]
    public static extern int DwmIsCompositionEnabled([Out] out int pfEnabled);

    [DllImport(Libraries.Dwmapi, PreserveSig = false)]
    public static extern void DwmExtendFrameIntoClientArea([In] IntPtr hWnd, [In] ref UxTheme.MARGINS pMarInset);

    [DllImport(Libraries.Dwmapi)]
    public static extern void DwmGetCompositionTimingInfo([In] IntPtr hWnd, [In] ref DWM_TIMING_INFO pTimingInfo);

    [DllImport(Libraries.Dwmapi, PreserveSig = false)]
    public static extern void DwmInvalidateIconicBitmaps([In] IntPtr hWnd);

    [DllImport(Libraries.Dwmapi, PreserveSig = false)]
    public static extern void DwmSetIconicThumbnail([In] IntPtr hWnd, [In] IntPtr hbmp, [In] DWM_SIT dwSITFlags);

    [DllImport(Libraries.Dwmapi, PreserveSig = false)]
    public static extern int DwmSetIconicLivePreviewBitmap([In] IntPtr hWnd, [In] IntPtr hbmp,
        [In, Optional] WinDef.POINT pptClient, [In] DWM_SIT dwSITFlags);

    [DllImport(Libraries.Dwmapi)]
    public static extern int DwmSetWindowAttribute([In] IntPtr hWnd, [In] int dwAttribute,
        [In] ref int pvAttribute,
        [In] int cbAttribute);

    [DllImport(Libraries.Dwmapi)]
    public static extern int DwmSetWindowAttribute([In] IntPtr hWnd, [In] DWMWINDOWATTRIBUTE dwAttribute,
        [In] ref int pvAttribute,
        [In] int cbAttribute);

    [DllImport(Libraries.Dwmapi)]
    public static extern int DwmGetWindowAttribute([In] IntPtr hWnd, [In] DWMWINDOWATTRIBUTE dwAttributeToGet,
           [In] ref int pvAttributeValue,
           [In] int cbAttribute);

    [DllImport(Libraries.Dwmapi)]
    public static extern int DwmGetWindowAttribute([In] IntPtr hWnd, [In] int dwAttributeToGet,
            [In] ref int pvAttributeValue,
            [In] int cbAttribute);

    [DllImport(Libraries.Dwmapi, EntryPoint = "#127", PreserveSig = false, CharSet = CharSet.Unicode)]
    public static extern void DwmGetColorizationParameters([Out] out DWMCOLORIZATIONPARAMS dwParameters);
}
