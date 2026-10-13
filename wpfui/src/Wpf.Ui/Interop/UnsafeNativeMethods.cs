using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Appearance;

namespace Wpf.Ui.Interop;

public static class UnsafeNativeMethods
{
    #region Window Corners

    public static bool ApplyWindowCornerPreference(Window window, WindowCornerPreference cornerPreference)
        => GetHandle(window, out IntPtr windowHandle) && ApplyWindowCornerPreference(windowHandle, cornerPreference);

    public static bool ApplyWindowCornerPreference(IntPtr handle, WindowCornerPreference cornerPreference)
    {
        if (handle == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(handle))
            return false;

        int pvAttribute = (int)UnsafeReflection.Cast(cornerPreference);

        Dwmapi.DwmSetWindowAttribute(
            handle,
            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
            ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

        return true;
    }

    #endregion

    #region Window Immersive Dark Mode

    public static bool RemoveWindowDarkMode(Window window)
        => GetHandle(window, out IntPtr windowHandle) && RemoveWindowDarkMode(windowHandle);

    public static bool RemoveWindowDarkMode(IntPtr handle)
    {
        if (handle == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(handle))
            return false;

        var pvAttribute = 0x0;
        var dwAttribute = Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE;

        if (!Win32.Utilities.IsOSWindows11Insider1OrNewer)
            dwAttribute = Dwmapi.DWMWINDOWATTRIBUTE.DMWA_USE_IMMERSIVE_DARK_MODE_OLD;

        Dwmapi.DwmSetWindowAttribute(
            handle,
            dwAttribute,
            ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

        return true;
    }

    public static bool ApplyWindowDarkMode(Window window)
        => GetHandle(window, out IntPtr windowHandle) && ApplyWindowDarkMode(windowHandle);

    public static bool ApplyWindowDarkMode(IntPtr handle)
    {
        if (handle == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(handle))
            return false;

        var pvAttribute = 0x1;
        var dwAttribute = Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE;

        if (!Win32.Utilities.IsOSWindows11Insider1OrNewer)
            dwAttribute = Dwmapi.DWMWINDOWATTRIBUTE.DMWA_USE_IMMERSIVE_DARK_MODE_OLD;

        Dwmapi.DwmSetWindowAttribute(
            handle,
            dwAttribute,
            ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

        return true;
    }

    #endregion

    #region Window Titlebar

    public static bool RemoveWindowTitlebar(Window window)
    {
        if (window == null)
            return false;

        if (window.IsLoaded)
            return GetHandle(window, out IntPtr windowHandle) && RemoveWindowTitlebar(windowHandle);

        window.Loaded += (sender, _) =>
        {
            GetHandle(sender as Window, out IntPtr windowHandle);
            RemoveWindowTitlebar(windowHandle);
        };

        return true;
    }

    public static bool RemoveWindowTitlebar(IntPtr handle)
    {
        if (handle == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(handle))
            return false;

        var windowStyleLong = User32.GetWindowLong(handle, User32.GWL.GWL_STYLE);
        windowStyleLong &= ~(int)User32.WS.SYSMENU;

        var result = User32.SetWindowLong(handle, User32.GWL.GWL_STYLE, windowStyleLong);

        return result > 0x0;
    }

    #endregion

    #region Window Backdrop Effect

    public static bool ApplyWindowBackdrop(Window window, BackgroundType backgroundType)
        => GetHandle(window, out IntPtr windowHandle) && ApplyWindowBackdrop(windowHandle, backgroundType);

    public static bool ApplyWindowBackdrop(IntPtr handle, BackgroundType backgroundType)
    {
        if (handle == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(handle))
            return false;

        var backdropPvAttribute = (int)UnsafeReflection.Cast(backgroundType);

        if (backdropPvAttribute == (int)Dwmapi.DWMSBT.DWMSBT_DISABLE)
            return false;

        Dwmapi.DwmSetWindowAttribute(
            handle,
            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
            ref backdropPvAttribute,
            Marshal.SizeOf(typeof(int)));

        return true;
    }

    public static bool RemoveWindowBackdrop(Window window)
        => GetHandle(window, out IntPtr windowHandle) && RemoveWindowBackdrop(windowHandle);

    public static bool RemoveWindowBackdrop(IntPtr handle)
    {
        if (handle == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(handle))
            return false;

        var pvAttribute = 0x0;
        var backdropPvAttribute = (int)Dwmapi.DWMSBT.DWMSBT_DISABLE;

        Dwmapi.DwmSetWindowAttribute(
            handle,
            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT,
            ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

        Dwmapi.DwmSetWindowAttribute(
            handle,
            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
            ref backdropPvAttribute,
            Marshal.SizeOf(typeof(int)));

        return true;
    }

    public static bool IsWindowHasBackdrop(Window window, BackgroundType backdropType)
        => GetHandle(window, out IntPtr windowHandle) && IsWindowHasBackdrop(windowHandle, backdropType);

    public static bool IsWindowHasBackdrop(IntPtr handle, BackgroundType backdropType)
    {
        if (!User32.IsWindow(handle))
            return false;

        var pvAttribute = 0x0;

        Dwmapi.DwmGetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

        return pvAttribute == (int)UnsafeReflection.Cast(backdropType);
    }

    #endregion Window Backdrop Effect

    #region Initial Windows 11 Mica

    public static bool IsWindowHasLegacyMica(Window window)
        => GetHandle(window, out IntPtr windowHandle) && IsWindowHasLegacyMica(windowHandle);

    public static bool IsWindowHasLegacyMica(IntPtr handle)
    {
        if (!User32.IsWindow(handle))
            return false;

        var pvAttribute = 0x0;

        Dwmapi.DwmGetWindowAttribute(handle, Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT, ref pvAttribute,
            Marshal.SizeOf(typeof(int)));

        return pvAttribute == 0x1;
    }

    public static bool ApplyWindowLegacyMicaEffect(Window window)
        => GetHandle(window, out IntPtr windowHandle) && ApplyWindowLegacyMicaEffect(windowHandle);

    public static bool ApplyWindowLegacyMicaEffect(IntPtr handle)
    {
        var backdropPvAttribute = 0x1;

        Dwmapi.DwmSetWindowAttribute(
            handle,
            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT,
            ref backdropPvAttribute,
            Marshal.SizeOf(typeof(int)));

        return true;
    }

    #endregion

    #region Window Legacy Acrylic

    public static bool ApplyWindowLegacyAcrylicEffect(Window window)
        => GetHandle(window, out IntPtr windowHandle) && ApplyWindowLegacyAcrylicEffect(windowHandle);

    public static bool ApplyWindowLegacyAcrylicEffect(IntPtr handle)
    {
        var accentPolicy = new Interop.User32.ACCENT_POLICY
        {
            nAccentState = User32.ACCENT_STATE.ACCENT_ENABLE_ACRYLICBLURBEHIND,
            nColor = 0x990000 & 0xFFFFFF
        };

        var accentStructSize = Marshal.SizeOf(accentPolicy);
        var accentPtr = Marshal.AllocHGlobal(accentStructSize);
        
        Marshal.StructureToPtr(accentPolicy, accentPtr, false);

        var data = new User32.WINCOMPATTRDATA
        {
            Attribute = User32.WCA.WCA_ACCENT_POLICY,
            SizeOfData = accentStructSize,
            Data = accentPtr
        };

        User32.SetWindowCompositionAttribute(handle, ref data);

        Marshal.FreeHGlobal(accentPtr);

        return true;
    }

    #endregion

    #region Windows Aero

    public static bool ApplyWindowAeroEffect(Window window)
        => GetHandle(window, out IntPtr windowHandle) && ApplyWindowAeroEffect(windowHandle);

    public static bool ApplyWindowAeroEffect(IntPtr handle)
    {
        var accentPolicy = new Interop.User32.ACCENT_POLICY
        {
            nAccentState = User32.ACCENT_STATE.ACCENT_ENABLE_BLURBEHIND
        };

        var accentStructSize = Marshal.SizeOf(accentPolicy);
        var accentPtr = Marshal.AllocHGlobal(accentStructSize);

        Marshal.StructureToPtr(accentPolicy, accentPtr, false);

        var data = new User32.WINCOMPATTRDATA
        {
            Attribute = User32.WCA.WCA_ACCENT_POLICY,
            SizeOfData = accentStructSize,
            Data = accentPtr
        };

        User32.SetWindowCompositionAttribute(handle, ref data);

        Marshal.FreeHGlobal(accentPtr);

        return true;
    }

    #endregion

    #region DMWA Colorization

    public static Color GetDwmColor()
    {
        byte[] values;

        try
        {
            Dwmapi.DwmGetColorizationParameters(out var dwmParams);
            values = BitConverter.GetBytes(dwmParams.clrColor);
        }
        catch (Exception)
        {
            values = new byte[]
            {
                255,
                255,
                255
            };
        }

        return Color.FromArgb(
            255,
            values[2],
            values[1],
            values[0]
        );
    }

    #endregion

    #region Taskbar

    internal static bool SetTaskbarState(IntPtr hWnd, ShObjIdl.TBPFLAG taskbarFlag)
    {
        if (hWnd == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(hWnd))
            return false;

        var taskbarList = new ShObjIdl.CTaskbarList() as ShObjIdl.ITaskbarList4;

        if (taskbarList == null)
            return false;

        taskbarList.HrInit();
        taskbarList.SetProgressState(hWnd, taskbarFlag);

        return true;
    }

    internal static bool SetTaskbarValue(IntPtr hWnd, ShObjIdl.TBPFLAG taskbarFlag, int current, int total)
    {
        if (hWnd == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(hWnd))
            return false;

        var taskbarList = new ShObjIdl.CTaskbarList() as ShObjIdl.ITaskbarList4;

        if (taskbarList == null)
            return false;

        taskbarList.HrInit();
        taskbarList.SetProgressState(hWnd, taskbarFlag);

        if (taskbarFlag != ShObjIdl.TBPFLAG.TBPF_INDETERMINATE && taskbarFlag != ShObjIdl.TBPFLAG.TBPF_NOPROGRESS)
            taskbarList.SetProgressValue(
                hWnd,
                Convert.ToUInt64(current),
                Convert.ToUInt64(total));

        return true;
    }

    #endregion

    #region Client area and Title Bar

    public static bool RemoveWindowCaption(Window window)
    {
        if (window == null)
            return false;

        var windowHandle = new WindowInteropHelper(window).Handle;

        return RemoveWindowCaption(windowHandle);
    }

    public static bool RemoveWindowCaption(IntPtr hWnd)
    {
        if (hWnd == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(hWnd))
            return false;

        var wtaOptions = new UxTheme.WTA_OPTIONS()
        {
            dwFlags = UxTheme.WTNCA.NODRAWCAPTION,
            dwMask = UxTheme.WTNCA.VALIDBITS
        };

        UxTheme.SetWindowThemeAttribute(
            hWnd,
            UxTheme.WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT,
            ref wtaOptions,
            (uint)Marshal.SizeOf(typeof(UxTheme.WTA_OPTIONS)));

        return true;
    }

    public static bool ExtendClientAreaIntoTitleBar(Window window)
    {
        if (window == null)
            return false;

        var windowHandle = new WindowInteropHelper(window).Handle;

        return ExtendClientAreaIntoTitleBar(windowHandle);
    }

    public static bool ExtendClientAreaIntoTitleBar(IntPtr hWnd)
    {

        if (hWnd == IntPtr.Zero)
            return false;

        if (!User32.IsWindow(hWnd))
            return false;

        var wtaOptions = new UxTheme.WTA_OPTIONS()
        {
            dwFlags = (UxTheme.WTNCA.NODRAWCAPTION | UxTheme.WTNCA.NODRAWICON | UxTheme.WTNCA.NOSYSMENU),
            dwMask = UxTheme.WTNCA.VALIDBITS
        };

        Interop.UxTheme.SetWindowThemeAttribute(
            hWnd,
            UxTheme.WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT,
            ref wtaOptions,
            (uint)Marshal.SizeOf(typeof(UxTheme.WTA_OPTIONS)));

        var windowDpi = Ui.Dpi.DpiHelper.GetWindowDpi(hWnd);

        var deviceGlassThickness = Ui.Dpi.DpiHelper.LogicalThicknessToDevice(
            new Thickness(-1, -1, -1, -1),
            windowDpi.DpiScaleX,
            windowDpi.DpiScaleY);

        var dwmMargin = new UxTheme.MARGINS
        {
            cxLeftWidth = (int)Math.Ceiling(deviceGlassThickness.Left),
            cxRightWidth = (int)Math.Ceiling(deviceGlassThickness.Right),
            cyTopHeight = (int)Math.Ceiling(deviceGlassThickness.Top),
            cyBottomHeight = (int)Math.Ceiling(deviceGlassThickness.Bottom),
        };

        Interop.Dwmapi.DwmExtendFrameIntoClientArea(hWnd, ref dwmMargin);

        Interop.User32.SetWindowRgn(hWnd, IntPtr.Zero,
            Interop.User32.IsWindowVisible(hWnd));

        return true;
    }

    public static void RestoreDefaultClientArea(Window window)
    {
    }

    #endregion Client area and Title bar

    public static bool IsCompositionEnabled()
    {
        Dwmapi.DwmIsCompositionEnabled(out var isEnabled);

        return isEnabled == 0x1;
    }

    public static bool IsValidWindow(IntPtr hWnd)
    {
        return User32.IsWindow(hWnd);
    }

    private static bool GetHandle(Window window, out IntPtr windowHandle)
    {
        windowHandle = new WindowInteropHelper(window).Handle;

        return windowHandle != IntPtr.Zero;
    }
}
