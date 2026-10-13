using System;
using System.Windows;
using System.Windows.Markup;
using Wpf.Ui.Appearance;

namespace Wpf.Ui.Markup;

[Localizability(LocalizationCategory.Ignore)]
[Ambient]
[UsableDuringInitialization(true)]
public class ControlsDictionary : ResourceDictionary
{
    public ControlsDictionary()
        => Source = new Uri($"{AppearanceData.LibraryDictionariesUri}{AppearanceData.LibraryMainDictionary}.xaml", UriKind.Absolute);
}
