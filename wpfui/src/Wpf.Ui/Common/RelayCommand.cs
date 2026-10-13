using System;
using System.Windows.Input;

namespace Wpf.Ui.Common;

public sealed class RelayCommand : IRelayCommand
{
    readonly Action<object> _execute;

    readonly Func<bool> _canExecute;

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public RelayCommand(Action execute) : this(execute, null)
    {
    }

    public RelayCommand(Action<object> execute) : this(execute, null)
    {
    }

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
        if (execute == null)
            throw new ArgumentNullException("execute");

        _execute = p => execute();
        _canExecute = canExecute;
    }

    public RelayCommand(Action<object> execute, Func<bool> canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException("execute");
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute?.Invoke() ?? true;
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }
}
