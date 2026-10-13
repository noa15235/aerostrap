using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Wpf.Ui.Appearance;
using Wpf.Ui.Interop;

namespace Wpf.Ui.Extensions;

public static class WindowExtensions
{
    #region Styles

    public static Window ApplyBackdrop(this Window window, BackgroundType backgroundType)
    {
        Appearance.Background.Apply(window, backgroundType);

        return window;
    }

    public static Window RemoveAllStyles(this Window window)
    {
        if (window.IsLoaded)
            RemoveWindowStyles(window);
        else
            window.Loaded += WindowRemoveStylesOnLoaded;

        return window;
    }

    private static void WindowRemoveStylesOnLoaded(object sender, RoutedEventArgs e)
    {
        RemoveWindowStyles(sender as Window);
    }

    private static void RemoveWindowStyles(Window window)
    {
        var windowHandle = new WindowInteropHelper(window).Handle;

        Interop.User32.SetWindowLong(windowHandle, Interop.User32.GWL.GWL_STYLE, (long)Interop.User32.WS.BORDER);
    }

    #endregion

    #region Titlebar

    public static Window RemoveTitlebar(this Window window)
    {
        if (window.IsLoaded)
            UnsafeNativeMethods.RemoveWindowTitlebar(window);
        else
            window.Loaded += (sender, args) => UnsafeNativeMethods.RemoveWindowTitlebar(window);

        return window;
    }

    #endregion

    #region Background

    public static Window ApplyDefaultBackground(this Window window)
    {
        var applicationBackgroundRaw = Application.Current.Resources["ApplicationBackgroundColor"];

        if (applicationBackgroundRaw is not Color backgroundColor)
            return window;

        window.Background = new SolidColorBrush { Color = backgroundColor };

        return window;
    }

    #endregion

    #region Mica

    public static void ApplyBackgroundEffect(this Window window)
    {
    }

    #endregion

    #region Corners

    public static Window ApplyCorners(this Window window, WindowCornerPreference cornerPreference)
    {
        if (window.IsLoaded)
            UnsafeNativeMethods.ApplyWindowCornerPreference(window, cornerPreference);
        else
            window.Loaded += (sender, args) => UnsafeNativeMethods.ApplyWindowCornerPreference(window, cornerPreference);

        return window;
    }

    #endregion
}
