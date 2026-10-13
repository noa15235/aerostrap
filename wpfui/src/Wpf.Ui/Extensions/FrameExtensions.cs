using System.Windows;
using System.Windows.Controls;

namespace Wpf.Ui.Extensions;

public static class FrameExtensions
{
    public static object GetDataContext(this Frame frame)
    {
        return frame.Content is not FrameworkElement element ? null : element.DataContext;
    }

    public static void CleanNavigation(this Frame frame)
    {
        while (frame.CanGoBack)
            frame.RemoveBackEntry();
    }
}
