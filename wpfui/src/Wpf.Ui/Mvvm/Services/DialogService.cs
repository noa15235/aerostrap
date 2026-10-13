#nullable enable

using System;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Mvvm.Services;

public class DialogService : IDialogService
{
    private IDialogControl? _dialogControl;

    public void SetDialogControl(IDialogControl dialog)
    {
        _dialogControl = dialog;
    }

    public IDialogControl GetDialogControl()
    {
        if (_dialogControl is null)
            throw new InvalidOperationException(
                $"The ${typeof(DialogService)} cannot be used unless previously defined with {typeof(IDialogControl)}.{nameof(SetDialogControl)}().");

        return _dialogControl;
    }
}
