using System.Windows.Input;

namespace Wpf.Ui.Common;

public interface IRelayCommand : ICommand
{
    public new bool CanExecute(object parameter);

    public new void Execute(object parameter);
}
