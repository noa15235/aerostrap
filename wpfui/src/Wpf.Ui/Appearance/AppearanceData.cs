using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Interop;

namespace Wpf.Ui.Appearance;

internal static class AppearanceData
{
    public static List<IntPtr> ModifiedBackgroundHandles = new();

    public const string LibraryNamespace = "ui;";

    public const string LibraryMainDictionary = "Wpf.Ui";

    public const string LibraryThemeDictionariesUri = "pack://application:,,,/Wpf.Ui;component/Styles/Theme/";

    public const string LibraryDictionariesUri = "pack://application:,,,/Wpf.Ui;component/Styles/";

    public static SystemThemeType SystemTheme = SystemThemeType.Unknown;

    public static ThemeType ApplicationTheme = ThemeType.Unknown;

    public static void AddHandle(Window window)
    {
        AddHandle(new WindowInteropHelper(window).Handle);
    }

    public static void AddHandle(IntPtr hWnd)
    {
        if (!ModifiedBackgroundHandles.Contains(hWnd))
            ModifiedBackgroundHandles.Add(hWnd);
    }

    public static void RemoveHandle(Window window)
    {
        RemoveHandle(new WindowInteropHelper(window).Handle);
    }

    public static void RemoveHandle(IntPtr hWnd)
    {
        if (!ModifiedBackgroundHandles.Contains(hWnd))
            ModifiedBackgroundHandles.Remove(hWnd);
    }

    public static bool HasHandle(Window window)
    {
        return HasHandle(new WindowInteropHelper(window).Handle);
    }

    public static bool HasHandle(IntPtr hWnd)
    {
        return ModifiedBackgroundHandles.Contains(hWnd);
    }
}
