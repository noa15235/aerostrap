using System.Windows.Media;
using Wpf.Ui.Appearance;

namespace Wpf.Ui.Mvvm.Contracts;

public interface IThemeService
{
    ThemeType GetTheme();

    ThemeType GetSystemTheme();

    SystemThemeType GetNativeSystemTheme();

    bool SetTheme(ThemeType themeType);

    bool SetSystemAccent();

    bool SetAccent(Color accentColor);

    bool SetAccent(SolidColorBrush accentSolidBrush);
}

