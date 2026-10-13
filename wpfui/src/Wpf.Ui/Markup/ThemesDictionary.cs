using System;
using System.Windows;
using System.Windows.Markup;
using Wpf.Ui.Appearance;

namespace Wpf.Ui.Markup;

[Localizability(LocalizationCategory.Ignore)]
[Ambient]
[UsableDuringInitialization(true)]
public class ThemesDictionary : ResourceDictionary
{
    public ThemeType Theme
    {
        set
        {
            var themeName = value switch
            {
                ThemeType.Dark => "Dark",
                ThemeType.HighContrast => "HighContrast",
                _ => "Light"
            };

            Source = new Uri($"{AppearanceData.LibraryThemeDictionariesUri}{themeName}.xaml", UriKind.Absolute);
        }
    }
}

