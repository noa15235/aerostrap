using System;
using System.Windows;
using System.Windows.Interop;
using Wpf.Ui.Interop;

namespace Wpf.Ui.TaskBar;

public static class TaskBarProgress
{
    private static bool IsSupported()
    {
        return Win32.Utilities.IsOSWindows7OrNewer;
    }

    public static bool SetState(Window window, TaskBarProgressState taskBarProgressState)
    {
        if (window == null)
            return false;

        if (window.IsLoaded)
            return SetState(new WindowInteropHelper(window).Handle, taskBarProgressState);

        window.Loaded += (_, _) =>
        {
            SetState(new WindowInteropHelper(window).Handle, taskBarProgressState);
        };

        return true;
    }

    public static bool SetState(IntPtr hWnd, TaskBarProgressState taskBarProgressState)
    {
        if (!IsSupported())
            throw new Exception("Taskbar functions not available.");

        return UnsafeNativeMethods.SetTaskbarState(hWnd, UnsafeReflection.Cast(taskBarProgressState));
    }

    public static bool SetValue(Window window, TaskBarProgressState taskBarProgressState, int current)
    {
        if (current > 100)
            current = 100;

        if (current < 0)
            current = 0;

        return SetValue(window, taskBarProgressState, current, 100);
    }

    public static bool SetValue(Window window, TaskBarProgressState taskBarProgressState, int current, int total)
    {
        if (window == null)
            return false;

        if (window.IsLoaded)
            return SetValue(new WindowInteropHelper(window).Handle, taskBarProgressState, current, total);

        window.Loaded += (_, _) =>
        {
            SetValue(new WindowInteropHelper(window).Handle, taskBarProgressState, current, total);
        };

        return false;
    }

    public static bool SetValue(IntPtr hWnd, TaskBarProgressState taskBarProgressState, int current)
    {
        if (current > 100)
            current = 100;

        if (current < 0)
            current = 0;

        return SetValue(hWnd, taskBarProgressState, current, 100);
    }

    public static bool SetValue(IntPtr hWnd, TaskBarProgressState taskBarProgressState, int current, int total)
    {
        if (!IsSupported())
            throw new Exception("Taskbar functions not available.");

        return UnsafeNativeMethods.SetTaskbarValue(hWnd, UnsafeReflection.Cast(taskBarProgressState), current, total);
    }
}
