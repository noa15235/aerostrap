using System;
using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Services.Internal;

internal class NavigationServiceItem
{
    public string Tag { get; set; } = string.Empty;

    public bool Cache { get; set; } = false;

    public Type Type { get; set; } = null;

    public Uri Source { get; set; } = null;

    public object Instance { get; set; } = null;

    public bool SetContext(object dataContext)
    {
        if (!Cache)
            return false;

        if (Instance is not FrameworkElement)
            return false;

        ((FrameworkElement)Instance).DataContext = dataContext;

        return true;
    }

    public static NavigationServiceItem Create(INavigationItem navigationItem)
    {
        return new NavigationServiceItem
        {
            Tag = navigationItem.PageTag,
            Type = navigationItem.PageType,
            Source = navigationItem.AbsolutePageSource,
            Cache = navigationItem.Cache
        };
    }
}
