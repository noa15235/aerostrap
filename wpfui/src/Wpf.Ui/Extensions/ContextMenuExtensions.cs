using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using Wpf.Ui.Appearance;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Extensions;

internal static class ContextMenuExtensions
{
    public static void ApplyMica(this ContextMenu contextMenu)
    {
        contextMenu.Opened += ContextMenuOnOpened;
    }

    private static void ContextMenuOnOpened(object sender, RoutedEventArgs e)
    {
        if (sender is not ContextMenu contextMenu)
            return;

        var source = PresentationSource.FromVisual(contextMenu) as HwndSource;

        if (source == null)
            return;

        if (Theme.GetAppTheme() == ThemeType.Dark)
            UnsafeNativeMethods.ApplyWindowDarkMode(source.Handle);

    }
}
