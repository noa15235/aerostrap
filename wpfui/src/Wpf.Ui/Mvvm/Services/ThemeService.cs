using System;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Mvvm.Services;

public partial class ThemeService : IThemeService
{
    public virtual ThemeType GetTheme()
        => Wpf.Ui.Appearance.Theme.GetAppTheme();

    public virtual SystemThemeType GetNativeSystemTheme()
        => Wpf.Ui.Appearance.Theme.GetSystemTheme();

    public virtual ThemeType GetSystemTheme()
    {
        var systemTheme = Wpf.Ui.Appearance.Theme.GetSystemTheme();

        return systemTheme switch
        {
            SystemThemeType.Light => ThemeType.Light,
            SystemThemeType.Dark => ThemeType.Dark,
            SystemThemeType.Glow => ThemeType.Dark,
            SystemThemeType.CapturedMotion => ThemeType.Dark,
            SystemThemeType.Sunrise => ThemeType.Light,
            SystemThemeType.Flow => ThemeType.Light,
            _ => ThemeType.Unknown
        };
    }

    public virtual bool SetTheme(ThemeType themeType)
    {
        if (Wpf.Ui.Appearance.Theme.GetAppTheme() == themeType)
            return false;

        Wpf.Ui.Appearance.Theme.Apply(themeType);

        return true;
    }

    public bool SetSystemAccent()
    {
        Wpf.Ui.Appearance.Accent.ApplySystemAccent();

        return true;
    }

    public bool SetAccent(Color accentColor)
    {
        Wpf.Ui.Appearance.Accent.Apply(accentColor);

        return true;
    }

    public bool SetAccent(SolidColorBrush accentSolidBrush)
    {
        var color = accentSolidBrush.Color;
        color.A = (byte)Math.Round(accentSolidBrush.Opacity * Byte.MaxValue);

        Wpf.Ui.Appearance.Accent.Apply(color);

        return true;
    }
}
