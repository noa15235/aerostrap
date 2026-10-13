using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace Wpf.Ui.Markup;

[TypeConverter(typeof(DynamicResourceExtensionConverter))]
[MarkupExtensionReturnType(typeof(object))]
public class ThemeResourceExtension : DynamicResourceExtension
{
    public ThemeResourceExtension()
    {
        ResourceKey = ThemeResource.ApplicationBackgroundBrush.ToString();
    }

    public ThemeResourceExtension(ThemeResource resourceKey)
    {
        if (resourceKey == ThemeResource.Unknown)
            throw new ArgumentNullException(nameof(resourceKey));

        ResourceKey = resourceKey.ToString();
    }
}
