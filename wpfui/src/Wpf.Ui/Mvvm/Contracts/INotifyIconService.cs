using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf.Ui.Mvvm.Contracts;

public interface INotifyIconService
{
    public int Id { get; }

    public bool IsRegistered { get; }

    public string TooltipText { get; set; }

    ContextMenu ContextMenu { get; set; }

    public ImageSource Icon { get; set; }

    public bool Register();

    public bool Unregister();

    public void SetParentWindow(Window window);
}

