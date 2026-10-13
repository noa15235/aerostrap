using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls.Interfaces;

public interface IDialogControl
{
    public enum ButtonPressed
    {
        None,

        Left,

        Right
    }

    bool IsShown { get; }

    object Footer { get; set; }

    double DialogWidth { get; set; }

    double DialogHeight { get; set; }

    string Title { get; set; }

    string Message { get; set; }

    object Content { get; set; }

    string ButtonLeftName { get; set; }

    string ButtonRightName { get; set; }

    event RoutedEventHandler ButtonRightClick;

    event RoutedEventHandler ButtonLeftClick;

    public event RoutedDialogEvent Opened;

    public event RoutedDialogEvent Closed;

    bool Show();

    bool Show(string title, string message);

    Task<ButtonPressed> ShowAndWaitAsync();

    Task<ButtonPressed> ShowAndWaitAsync(bool hideOnClick);

    Task<ButtonPressed> ShowAndWaitAsync(string title, string message);

    Task<ButtonPressed> ShowAndWaitAsync(string title, string message, bool hideOnClick);

    bool Hide();
}
