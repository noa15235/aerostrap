using System.Windows;
using Wpf.Ui.Controls.Interfaces;

namespace Wpf.Ui.Mvvm.Interfaces;

public interface IViewModel
{
    public void OnMounted(FrameworkElement parentElement);
}
