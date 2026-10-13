using System.Threading.Tasks;
using Wpf.Ui.Common;

namespace Wpf.Ui.Controls.Interfaces;

public interface ISnackbarControl
{
    bool IsShown { get; }

    int Timeout { get; set; }

    string Title { get; set; }

    string Message { get; set; }

    bool CloseButtonEnabled { get; set; }

    event RoutedSnackbarEvent Opened;

    event RoutedSnackbarEvent Closed;

    bool Show();

    bool Show(string title);

    bool Show(string title, string message);

    bool Show(string title, string message, SymbolRegular icon);

    bool Show(string title, string message, SymbolRegular icon, ControlAppearance appearance);

    Task<bool> ShowAsync();

    Task<bool> ShowAsync(string title);

    Task<bool> ShowAsync(string title, string message);

    Task<bool> ShowAsync(string title, string message, SymbolRegular icon);

    Task<bool> ShowAsync(string title, string message, SymbolRegular icon, ControlAppearance appearance);

    bool Hide();

    Task<bool> HideAsync();
}
