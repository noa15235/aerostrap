using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Mvvm.Contracts;

public interface ISnackbarService
{
    bool IsShown { get; }

    int Timeout { get; set; }

    void SetSnackbarControl(ISnackbarControl snackbar);

    ISnackbarControl GetSnackbarControl();

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
