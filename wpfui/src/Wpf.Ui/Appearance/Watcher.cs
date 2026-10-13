using System;
using System.Windows;
using System.Windows.Interop;

namespace Wpf.Ui.Appearance;

public sealed class Watcher
{
    public BackgroundType BackgroundEffect { get; set; } = BackgroundType.Unknown;

    public bool UpdateAccents { get; set; } = false;

    public bool ForceBackground { get; set; } = false;

    public static void Watch(Window window, BackgroundType backgroundEffect = BackgroundType.Mica,
        bool updateAccents = true, bool forceBackground = false)
    {
        if (window == null)
            return;

        if (window.IsLoaded)
        {
            IntPtr hwnd =
                (hwnd = new WindowInteropHelper(window).Handle) == IntPtr.Zero
                    ? throw new InvalidOperationException("Could not get window handle.")
                    : hwnd;

            var watcher = new Watcher(hwnd, backgroundEffect, updateAccents, forceBackground);

            var currentSystemTheme = SystemTheme.GetTheme();
            watcher.UpdateThemes(currentSystemTheme);

            return;
        }

        window.Loaded += (sender, args) =>
        {
            IntPtr hwnd =
                (hwnd = new WindowInteropHelper(window).Handle) == IntPtr.Zero
                    ? throw new InvalidOperationException("Could not get window handle.")
                    : hwnd;

            var watcher = new Watcher(hwnd, backgroundEffect, updateAccents, forceBackground);

            var currentSystemTheme = SystemTheme.GetTheme();
            watcher.UpdateThemes(currentSystemTheme);
        };
    }

    public Watcher(IntPtr hWnd, BackgroundType backgroundEffect, bool updateAccents, bool forceBackground)
    {
        var hWndSource = HwndSource.FromHwnd(hWnd);

        BackgroundEffect = backgroundEffect;
        ForceBackground = forceBackground;
        UpdateAccents = updateAccents;

        hWndSource?.AddHook(WndProc);
    }

    private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg != (int)Interop.User32.WM.WININICHANGE)
            return IntPtr.Zero;

        var currentSystemTheme = SystemTheme.GetTheme();
        UpdateThemes(currentSystemTheme);

        return IntPtr.Zero;
    }

    private void UpdateThemes(SystemThemeType systemTheme)
    {
        AppearanceData.SystemTheme = systemTheme;

        var themeToSet = ThemeType.Light;

        if (systemTheme is SystemThemeType.Dark or SystemThemeType.CapturedMotion or SystemThemeType.Glow)
            themeToSet = ThemeType.Dark;

        Theme.Apply(themeToSet, BackgroundEffect, UpdateAccents, ForceBackground);

#if DEBUG
        System.Diagnostics.Debug.WriteLine($"INFO | {typeof(Watcher)} changed the app theme.", "Wpf.Ui.Watcher");
        System.Diagnostics.Debug.WriteLine($"INFO | Current accent: {Accent.SystemAccent}", "Wpf.Ui.Watcher");
        System.Diagnostics.Debug.WriteLine($"INFO | Current app theme: {AppearanceData.ApplicationTheme}",
            "Wpf.Ui.Watcher");
        System.Diagnostics.Debug.WriteLine($"INFO | Current system theme: {AppearanceData.SystemTheme}",
            "Wpf.Ui.Watcher");
#endif
    }
}
