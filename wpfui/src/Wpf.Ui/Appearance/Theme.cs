using System;
using System.Windows;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Appearance;

public static class Theme
{
    public static event ThemeChangedEvent Changed;

    public static bool IsHighContrast() => AppearanceData.ApplicationTheme == ThemeType.HighContrast;

    public static bool IsSystemHighContrast() => SystemTheme.HighContrast;

    public static void Apply(ThemeType themeType, BackgroundType backgroundEffect = BackgroundType.Mica,
        bool updateAccent = true, bool forceBackground = false)
    {
        if (updateAccent)
            Accent.Apply(
                Accent.GetColorizationColor(),
                themeType,
                false
            );

        if (themeType == ThemeType.Unknown || themeType == AppearanceData.ApplicationTheme)
            return;

        var appDictionaries = new ResourceDictionaryManager(AppearanceData.LibraryNamespace);

        var themeDictionaryName = "Light";

        switch (themeType)
        {
            case ThemeType.Dark:
                themeDictionaryName = "Dark";
                break;
        }

        var isUpdated = appDictionaries.UpdateDictionary(
            "theme",
            new Uri(
                AppearanceData.LibraryThemeDictionariesUri + themeDictionaryName + ".xaml",
                UriKind.Absolute
            )
        );

#if DEBUG
        System.Diagnostics.Debug.WriteLine(
            $"INFO | {typeof(Theme)} tries to update theme to {themeDictionaryName} ({themeType}): {isUpdated}",
            "Wpf.Ui.Theme");
#endif
        if (!isUpdated)
            return;

        AppearanceData.ApplicationTheme = themeType;

        Changed?.Invoke(themeType, Accent.SystemAccent);

        UpdateBackground(themeType, backgroundEffect, forceBackground);
    }

    public static ThemeType GetAppTheme()
    {
        if (AppearanceData.ApplicationTheme == ThemeType.Unknown)
            FetchApplicationTheme();

        return AppearanceData.ApplicationTheme;
    }

    public static SystemThemeType GetSystemTheme()
    {
        if (AppearanceData.SystemTheme == SystemThemeType.Unknown)
            FetchSystemTheme();

        return AppearanceData.SystemTheme;
    }

    public static bool IsAppMatchesSystem()
    {
        var appTheme = GetAppTheme();
        var sysTheme = GetSystemTheme();

        return appTheme switch
        {
            ThemeType.Dark => sysTheme is SystemThemeType.Dark or SystemThemeType.CapturedMotion
                or SystemThemeType.Glow,
            ThemeType.Light => sysTheme is SystemThemeType.Light or SystemThemeType.Flow or SystemThemeType.Sunrise,
            _ => appTheme == ThemeType.HighContrast && SystemTheme.HighContrast
        };
    }

    public static bool IsMatchedDark()
    {
        var appTheme = GetAppTheme();
        var sysTheme = GetSystemTheme();

        if (appTheme != ThemeType.Dark)
            return false;

        return sysTheme is SystemThemeType.Dark or SystemThemeType.CapturedMotion or SystemThemeType.Glow;
    }

    public static bool IsMatchedLight()
    {
        var appTheme = GetAppTheme();
        var sysTheme = GetSystemTheme();

        if (appTheme != ThemeType.Light)
            return false;

        return sysTheme is SystemThemeType.Light or SystemThemeType.Flow or SystemThemeType.Sunrise;
    }

    public static bool ApplyDarkThemeToWindow(Window window)
    {
        if (window == null)
            return false;

        if (window.IsLoaded)
            return UnsafeNativeMethods.ApplyWindowDarkMode(window);

        window.Loaded += (sender, _) => UnsafeNativeMethods.ApplyWindowDarkMode(sender as Window);

        return true;
    }

    public static bool RemoveDarkThemeFromWindow(Window window)
    {
        if (window == null)
            return false;

        if (window.IsLoaded)
            return UnsafeNativeMethods.RemoveWindowDarkMode(window);

        window.Loaded += (sender, _) => UnsafeNativeMethods.RemoveWindowDarkMode(sender as Window);

        return true;
    }

    private static void FetchApplicationTheme()
    {
        var appDictionaries = new ResourceDictionaryManager(AppearanceData.LibraryNamespace);
        var themeDictionary = appDictionaries.GetDictionary("theme");

        if (themeDictionary == null)
            return;

        var themeUri = themeDictionary.Source.ToString().Trim().ToLower();

        if (themeUri.Contains("light"))
            AppearanceData.ApplicationTheme = ThemeType.Light;
        else if (themeUri.Contains("dark"))
            AppearanceData.ApplicationTheme = ThemeType.Dark;
        else if (themeUri.Contains("highcontrast"))
            AppearanceData.ApplicationTheme = ThemeType.HighContrast;
    }

    private static void FetchSystemTheme()
    {
        AppearanceData.SystemTheme = SystemTheme.GetTheme();
    }

    private static void UpdateBackground(ThemeType themeType,
        BackgroundType backgroundEffect = BackgroundType.Unknown, bool forceBackground = false)
    {
        Background.UpdateAll(themeType, backgroundEffect);

        if (!AppearanceData.HasHandle(Application.Current.MainWindow))
            Background.Apply(Application.Current.MainWindow, backgroundEffect, forceBackground);

    }
}
