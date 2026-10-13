#nullable enable

using System;
using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;

namespace Wpf.Ui.Mvvm.Services;

public class SnackbarService : ISnackbarService
{
    private ISnackbarControl? _snackbar;

    public bool IsShown
    {
        get
        {
            if (_snackbar == null)
                throw new InvalidOperationException(
                    $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

            return _snackbar.IsShown;
        }
    }

    public int Timeout
    {
        get => _snackbar?.Timeout ?? 0;
        set
        {
            if (_snackbar == null)
                throw new InvalidOperationException(
                    $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

            _snackbar.Timeout = value;
        }
    }

    public void SetSnackbarControl(ISnackbarControl snackbar)
    {
        _snackbar = snackbar;
    }

    public ISnackbarControl GetSnackbarControl()
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return _snackbar;
    }

    public bool Show()
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return _snackbar.Show();
    }

    public bool Show(string title)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return _snackbar.Show(title);
    }

    public bool Show(string title, string message)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return _snackbar.Show(title, message);
    }

    public bool Show(string title, string message, SymbolRegular icon)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return _snackbar.Show(title, message, icon);
    }

    public bool Show(string title, string message, SymbolRegular icon, ControlAppearance appearance)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return _snackbar.Show(title, message, icon, appearance);
    }

    public async Task<bool> ShowAsync()
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return await _snackbar.ShowAsync();
    }

    public async Task<bool> ShowAsync(string title)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return await _snackbar.ShowAsync(title);
    }

    public async Task<bool> ShowAsync(string title, string message)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return await _snackbar.ShowAsync(title, message);
    }

    public async Task<bool> ShowAsync(string title, string message, SymbolRegular icon)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return await _snackbar.ShowAsync(title, message, icon);
    }

    public async Task<bool> ShowAsync(string title, string message, SymbolRegular icon, ControlAppearance appearance)
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return await _snackbar.ShowAsync(title, message, icon, appearance);
    }

    public bool Hide()
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return _snackbar.Hide();
    }

    public async Task<bool> HideAsync()
    {
        if (_snackbar is null)
            throw new InvalidOperationException(
                $"The ${typeof(SnackbarService)} cannot be used unless previously defined with {typeof(ISnackbarService)}.{nameof(SetSnackbarControl)}().");

        return await _snackbar.HideAsync();
    }
}
